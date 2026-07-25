// Tipos espelhando os DTOs expostos pelo backend (GastosResidenciais.Api).

export type TipoTransacao = "Despesa" | "Receita";

export interface Pessoa {
  id: string;
  nome: string;
  idade: number;
  ehMenorDeIdade: boolean;
}

export interface Transacao {
  id: string;
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  pessoaId: string;
}

export interface CriarPessoaInput {
  nome: string;
  idade: number;
}

export interface CriarTransacaoInput {
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  pessoaId: string;
}

export interface TotalPessoa {
  pessoaId: string;
  nome: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

export interface ConsultaTotais {
  pessoas: TotalPessoa[];
  totalGeralReceitas: number;
  totalGeralDespesas: number;
  saldoGeral: number;
}