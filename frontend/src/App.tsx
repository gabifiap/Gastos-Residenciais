import { useCallback, useEffect, useState } from "react";
import { api } from "./api/client";
import type { Pessoa, Transacao } from "./api/types";
import { PessoasTab } from "./components/PessoasTab";
import { TransacoesTab } from "./components/TransacoesTab";
import { TotaisTab } from "./components/TotaisTab";

type Aba = "pessoas" | "transacoes" | "totais";

function App() {
  const [abaAtiva, setAbaAtiva] = useState<Aba>("pessoas");
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [versaoTotais, setVersaoTotais] = useState(0);

  const atualizarPessoas = useCallback(async () => {
    const dados = await api.pessoas.listar();
    setPessoas(dados);
    setVersaoTotais((v) => v + 1);
  }, []);

  const atualizarTransacoes = useCallback(async () => {
    const dados = await api.transacoes.listar();
    setTransacoes(dados);
    setVersaoTotais((v) => v + 1);
  }, []);

  useEffect(() => {
    atualizarPessoas();
  }, [atualizarPessoas]);

  return (
    <div className="min-h-screen bg-indigo-100 flex flex-col items-center p-8">
      <h1 className="text-3xl font-bold text-indigo-900 mb-6 text-center">
        Controle de Gastos Residenciais
      </h1>

      <div className="flex gap-2 mb-6">
        <button
          onClick={() => setAbaAtiva("pessoas")}
          className={`px-4 py-2 rounded ${abaAtiva === "pessoas" ? "bg-indigo-600 text-white" : "bg-white text-indigo-900"}`}
        >
          Pessoas
        </button>
        <button
          onClick={() => setAbaAtiva("transacoes")}
          className={`px-4 py-2 rounded ${abaAtiva === "transacoes" ? "bg-indigo-600 text-white" : "bg-white text-indigo-900"}`}
        >
          Transações
        </button>
        <button
          onClick={() => setAbaAtiva("totais")}
          className={`px-4 py-2 rounded ${abaAtiva === "totais" ? "bg-indigo-600 text-white" : "bg-white text-indigo-900"}`}
        >
          Totais
        </button>
      </div>

      {abaAtiva === "pessoas" && (
        <PessoasTab pessoas={pessoas} aoAtualizarPessoas={atualizarPessoas} />
      )}
      {abaAtiva === "transacoes" && (
        <TransacoesTab pessoas={pessoas} transacoes={transacoes} aoAtualizarTransacoes={atualizarTransacoes} />
      )}
      {abaAtiva === "totais" && <TotaisTab versao={versaoTotais} />}
    </div>
  );
}

export default App;