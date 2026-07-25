import { useEffect, useState } from "react";
import { api } from "../api/client";
import type { ConsultaTotais } from "../api/types";

interface TotaisTabProps {
  /**
   * Número que muda toda vez que uma pessoa ou transação é criada/removida.
   * Usado apenas para forçar o recarregamento dos totais quando algo muda
   * em outra aba (já que este componente não é notificado diretamente).
   */
  versao: number;
}

/**
 * Aba de Consulta de Totais.
 * Mostra, para cada pessoa cadastrada, o total de receitas, despesas e o
 * saldo (receitas - despesas). Ao final, exibe o total geral somando
 * todas as pessoas — exatamente como especificado no desafio.
 */
export function TotaisTab({ versao }: TotaisTabProps) {
  const [totais, setTotais] = useState<ConsultaTotais | null>(null);

  // Busca os totais sempre que "versao" mudar (ou seja, sempre que algo
  // relevante foi alterado em outra aba: nova pessoa, nova transação, etc).
  useEffect(() => {
    api.totais.consultar().then(setTotais);
  }, [versao]);

  // Enquanto os dados não chegaram, ou se não há ninguém cadastrado,
  // mostra uma mensagem simples em vez de uma tabela vazia.
  if (!totais || totais.pessoas.length === 0) {
    return <p className="text-indigo-900">Nenhuma pessoa cadastrada ainda.</p>;
  }

  return (
    <div className="bg-white rounded-lg shadow-lg p-6 w-full max-w-2xl">
      <h2 className="font-bold mb-4 text-center">Totais por pessoa</h2>
      <table className="w-full text-sm">
        <thead>
          <tr className="text-left border-b">
            <th className="py-2">Pessoa</th>
            <th className="py-2 text-right">Receitas</th>
            <th className="py-2 text-right">Despesas</th>
            <th className="py-2 text-right">Saldo</th>
          </tr>
        </thead>
        <tbody>
          {/* Uma linha por pessoa, com seus totais individuais */}
          {totais.pessoas.map((p) => (
            <tr key={p.pessoaId} className="border-b">
              <td className="py-2">{p.nome}</td>
              <td className="py-2 text-right text-green-600">
                R$ {p.totalReceitas.toFixed(2)}
              </td>
              <td className="py-2 text-right text-red-600">
                R$ {p.totalDespesas.toFixed(2)}
              </td>
              <td className="py-2 text-right font-bold">R$ {p.saldo.toFixed(2)}</td>
            </tr>
          ))}
        </tbody>
        <tfoot>
          {/* Linha final: total geral somando todas as pessoas */}
          <tr className="font-bold border-t-2">
            <td className="py-3">Total geral</td>
            <td className="py-3 text-right text-green-600">
              R$ {totais.totalGeralReceitas.toFixed(2)}
            </td>
            <td className="py-3 text-right text-red-600">
              R$ {totais.totalGeralDespesas.toFixed(2)}
            </td>
            <td className="py-3 text-right">R$ {totais.saldoGeral.toFixed(2)}</td>
          </tr>
        </tfoot>
      </table>
    </div>
  );
}