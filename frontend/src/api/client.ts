import type {
  ConsultaTotais,
  CriarPessoaInput,
  CriarTransacaoInput,
  Pessoa,
  Transacao,
} from "./types";

const BASE_URL = "http://localhost:5003/api";

/** Erro lançado quando a API retorna uma resposta de erro (4xx/5xx). */
export class ApiError extends Error {}

async function requisitar<T>(caminho: string, opcoes?: RequestInit): Promise<T> {
  const resposta = await fetch(`${BASE_URL}${caminho}`, {
    ...opcoes,
    headers: { "Content-Type": "application/json", ...opcoes?.headers },
  });

  if (!resposta.ok) {
    const corpo = await resposta.json().catch(() => null);
    const mensagem = corpo?.mensagem ?? `Erro inesperado (HTTP ${resposta.status}).`;
    throw new ApiError(mensagem);
  }

  if (resposta.status === 204) return undefined as T;
  return (await resposta.json()) as T;
}

export const api = {
  pessoas: {
    listar: () => requisitar<Pessoa[]>("/pessoas"),
    criar: (dados: CriarPessoaInput) =>
      requisitar<Pessoa>("/pessoas", { method: "POST", body: JSON.stringify(dados) }),
    remover: (id: string) => requisitar<void>(`/pessoas/${id}`, { method: "DELETE" }),
  },
  transacoes: {
    listar: () => requisitar<Transacao[]>("/transacoes"),
    criar: (dados: CriarTransacaoInput) =>
      requisitar<Transacao>("/transacoes", { method: "POST", body: JSON.stringify(dados) }),
  },
  totais: {
    consultar: () => requisitar<ConsultaTotais>("/totais"),
  },
};