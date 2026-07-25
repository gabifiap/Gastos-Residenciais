import { useEffect, useState } from "react";
import { api, ApiError } from "../api/client";
import type { Pessoa, Transacao, TipoTransacao } from "../api/types";

interface TransacoesTabProps {
  /** Lista de pessoas cadastradas, usada para popular o seletor do formulário. */
  pessoas: Pessoa[];
  /** Lista de transações já cadastradas. */
  transacoes: Transacao[];
  /** Função que recarrega a lista de transações a partir da API. */
  aoAtualizarTransacoes: () => Promise<void>;
}

/**
 * Aba de Cadastro de Transações.
 * Permite criar novas transações (despesa ou receita) associadas a uma
 * pessoa, e lista as já cadastradas. Não permite editar nem excluir
 * transações, conforme especificado no desafio.
 *
 * Regra de negócio importante: se a pessoa selecionada for menor de
 * idade (menor de 18 anos), apenas despesas podem ser cadastradas.
 * Essa regra é validada tanto na tela (avisando o usuário e desabilitando
 * a opção "Receita") quanto no backend (que é a fonte da verdade —
 * a validação na tela é só para dar um feedback mais rápido ao usuário).
 */
export function TransacoesTab({
  pessoas,
  transacoes,
  aoAtualizarTransacoes,
}: TransacoesTabProps) {
  const [descricao, setDescricao] = useState("");
  const [valor, setValor] = useState("");
  const [tipo, setTipo] = useState<TipoTransacao>("Despesa");
  const [pessoaId, setPessoaId] = useState("");
  const [erro, setErro] = useState<string | null>(null);

  // Busca as transações assim que a aba é exibida pela primeira vez.
  useEffect(() => {
    aoAtualizarTransacoes();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  // Encontra o objeto completo da pessoa selecionada no dropdown,
  // para podermos checar se ela é menor de idade.
  const pessoaSelecionada = pessoas.find((p) => p.id === pessoaId);

  /** Envia o formulário: cria uma nova transação via API. */
  async function handleSubmit(evento: React.FormEvent) {
    evento.preventDefault();
    setErro(null);

    try {
      await api.transacoes.criar({ descricao, valor: Number(valor), tipo, pessoaId });
      // Limpa os campos de texto após o sucesso (mantém a pessoa selecionada,
      // já que é comum lançar várias transações seguidas para a mesma pessoa).
      setDescricao("");
      setValor("");
      await aoAtualizarTransacoes();
    } catch (e) {
      // Se o backend rejeitar (ex.: menor de idade tentando cadastrar receita),
      // a mensagem de erro explicativa da API é exibida aqui.
      setErro(e instanceof ApiError ? e.message : "Erro ao cadastrar.");
    }
  }

  /** Busca o nome de uma pessoa a partir do seu id, para exibir na lista. */
  function nomeDaPessoa(id: string): string {
    return pessoas.find((p) => p.id === id)?.nome ?? "Pessoa removida";
  }

  return (
    <div className="w-full max-w-sm flex flex-col items-center">
      <form onSubmit={handleSubmit} className="bg-white p-6 rounded-lg shadow-lg mb-6 w-full">
        <h2 className="font-bold mb-3 text-center">Nova transação</h2>

        <select
          value={pessoaId}
          onChange={(e) => setPessoaId(e.target.value)}
          className="w-full border rounded px-3 py-2 mb-1"
        >
          <option value="">Selecione a pessoa</option>
          {pessoas.map((p) => (
            <option key={p.id} value={p.id}>
              {p.nome}
            </option>
          ))}
        </select>

        {/* Aviso visual: se a pessoa selecionada for menor de idade,
            lembra o usuário da restrição antes mesmo de tentar enviar. */}
        {pessoaSelecionada?.ehMenorDeIdade && (
          <p className="text-xs text-red-600 mb-3">
            Menor de idade: só pode cadastrar despesa.
          </p>
        )}

        <input
          value={descricao}
          onChange={(e) => setDescricao(e.target.value)}
          placeholder="Descrição"
          className="w-full border rounded px-3 py-2 mb-3 mt-3"
        />
        <input
          type="number"
          value={valor}
          onChange={(e) => setValor(e.target.value)}
          placeholder="Valor"
          className="w-full border rounded px-3 py-2 mb-3"
        />

        {/* Seletor de tipo (Despesa/Receita) como dois botões, em vez de
            um <select>, para deixar a escolha mais visual e rápida. */}
        <div className="flex gap-2 mb-3">
          <button
            type="button"
            onClick={() => setTipo("Despesa")}
            className={`flex-1 rounded py-2 ${
              tipo === "Despesa" ? "bg-red-600 text-white" : "border"
            }`}
          >
            Despesa
          </button>
          <button
            type="button"
            onClick={() => setTipo("Receita")}
            className={`flex-1 rounded py-2 ${
              tipo === "Receita" ? "bg-green-600 text-white" : "border"
            }`}
          >
            Receita
          </button>
        </div>

        {erro && <p className="text-red-600 text-sm mb-3 text-center">{erro}</p>}

        <button
          type="submit"
          className="bg-indigo-600 hover:bg-indigo-700 text-white rounded px-4 py-2 w-full transition-colors"
        >
          Cadastrar
        </button>
      </form>

      {/* Lista de transações já cadastradas */}
      <ul className="bg-white rounded-lg shadow-lg divide-y w-full">
        {transacoes.map((t) => (
          <li key={t.id} className="flex justify-between items-center p-3">
            <span>
              {nomeDaPessoa(t.pessoaId)} — {t.descricao}
            </span>
            {/* Verde para receita, vermelho para despesa, com sinal +/- */}
            <span className={t.tipo === "Receita" ? "text-green-600" : "text-red-600"}>
              {t.tipo === "Despesa" ? "-" : "+"} R$ {t.valor.toFixed(2)}
            </span>
          </li>
        ))}
      </ul>
    </div>
  );
}