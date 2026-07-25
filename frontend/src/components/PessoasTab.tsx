import { useEffect, useState } from "react";
import { api, ApiError } from "../api/client";
import type { Pessoa } from "../api/types";

interface PessoasTabProps {
  /** Lista de pessoas já cadastradas, controlada pelo componente pai (App.tsx). */
  pessoas: Pessoa[];
  /** Função que recarrega a lista de pessoas a partir da API. */
  aoAtualizarPessoas: () => Promise<void>;
}

/**
 * Aba de Cadastro de Pessoas.
 * Permite criar novas pessoas, listar as já cadastradas e removê-las.
 * Ao remover uma pessoa, o backend também apaga automaticamente todas
 * as transações associadas a ela (regra de negócio do desafio).
 */
export function PessoasTab({ pessoas, aoAtualizarPessoas }: PessoasTabProps) {
  // Estados do formulário: guardam o que o usuário está digitando.
  const [nome, setNome] = useState("");
  const [idade, setIdade] = useState("");
  // Guarda mensagens de erro vindas da API (ex.: validação falhou).
  const [erro, setErro] = useState<string | null>(null);

  // Assim que o componente é exibido pela primeira vez, busca a lista
  // atual de pessoas na API (o array vazio [] garante que rode só uma vez).
  useEffect(() => {
    aoAtualizarPessoas();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  /** Envia o formulário: cria uma nova pessoa via API. */
  async function handleSubmit(evento: React.FormEvent) {
    evento.preventDefault(); // evita que o navegador recarregue a página
    setErro(null);

    try {
      await api.pessoas.criar({ nome, idade: Number(idade) });
      // Limpa o formulário depois do sucesso.
      setNome("");
      setIdade("");
      // Recarrega a lista para mostrar a pessoa recém-criada.
      await aoAtualizarPessoas();
    } catch (e) {
      // Se a API rejeitar (ex.: nome vazio), mostra a mensagem de erro.
      setErro(e instanceof ApiError ? e.message : "Erro ao cadastrar.");
    }
  }

  /** Remove uma pessoa, após confirmação do usuário. */
  async function handleRemover(pessoa: Pessoa) {
    const confirmar = window.confirm(
      `Remover "${pessoa.nome}"? As transações dela também serão apagadas.`
    );
    if (!confirmar) return;

    await api.pessoas.remover(pessoa.id);
    await aoAtualizarPessoas();
  }

  return (
    <div className="w-full max-w-sm flex flex-col items-center">
      {/* Formulário de cadastro */}
      <form onSubmit={handleSubmit} className="bg-white p-6 rounded-lg shadow-lg mb-6 w-full">
        <h2 className="font-bold mb-3 text-center">Nova pessoa</h2>
        <input
          value={nome}
          onChange={(e) => setNome(e.target.value)}
          placeholder="Nome"
          className="w-full border rounded px-3 py-2 mb-3"
        />
        <input
          type="number"
          value={idade}
          onChange={(e) => setIdade(e.target.value)}
          placeholder="Idade"
          className="w-full border rounded px-3 py-2 mb-3"
        />
        {/* Mostra a mensagem de erro só quando existir uma */}
        {erro && <p className="text-red-600 text-sm mb-3 text-center">{erro}</p>}
        <button
          type="submit"
          className="bg-indigo-600 hover:bg-indigo-700 text-white rounded px-4 py-2 w-full transition-colors"
        >
          Cadastrar
        </button>
      </form>

      {/* Lista de pessoas já cadastradas */}
      <ul className="bg-white rounded-lg shadow-lg divide-y w-full">
        {pessoas.map((p) => (
          <li key={p.id} className="flex justify-between items-center p-3">
            <span>
              {p.nome} ({p.idade} anos)
              {/* Aviso visual quando a pessoa é menor de idade */}
              {p.ehMenorDeIdade && " — menor de idade"}
            </span>
            <button onClick={() => handleRemover(p)} className="text-red-600 text-sm">
              Remover
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}