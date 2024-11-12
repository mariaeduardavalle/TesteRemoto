﻿using System; 
 
public class Program 
{ 
    public static void Main() 
    { 
         /* using (var context = new LojaDbContext()) 
        { 
          var eletronicos = new Categoria { Nome = "Eletrônicos" }; 
            var roupas = new Categoria { Nome = "Roupas" }; 
            var alimentos = new Categoria { Nome = "Alimentos" }; 
 
            context.Categorias.AddRange(eletronicos, roupas, alimentos); 
 
            var smartphone = new Produto { Nome = "Smartphone", Preco = 1200.00m, DataAdicionado = DateTime.Now, Categoria = eletronicos }; 
            var laptop = new Produto { Nome = "Laptop", Preco = 3000.00m, DataAdicionado = DateTime.Now, Categoria = eletronicos }; 
            var camisa = new Produto { Nome = "Camisa", Preco = 80.00m, DataAdicionado = DateTime.Now, Categoria = roupas }; 
            var arroz = new Produto { Nome = "Arroz", Preco = 20.00m, DataAdicionado = DateTime.Now, Categoria = alimentos }; 
 
            context.Produtos.AddRange(smartphone, laptop, camisa, arroz); 
 
            var venda1 = new Venda { Produto = smartphone, Quantidade = 2, DataVenda = DateTime.Now }; 
            var venda2 = new Venda { Produto = laptop, Quantidade = 1, DataVenda = DateTime.Now }; 
            var venda3 = new Venda { Produto = camisa, Quantidade = 3, DataVenda = DateTime.Now }; 
 
            context.Vendas.AddRange(venda1, venda2, venda3); 
 
            context.SaveChanges(); 
        } 
 
        Console.WriteLine("Dados inseridos com sucesso!"); */

        using (var context = new LojaDbContext()){
            
            //a) Selecionar Todos os Produtos:
            Console.WriteLine("A) \n");
            var produtos = (from p in context.Produtos
                            select p).ToList();
            foreach(var produto in produtos){
                Console.WriteLine($"{produto.Nome} - R${produto.Preco}");
            }
            
            //b) Filtrar Produtos por Categoria “Eletrônicos”: 
            Console.WriteLine("\nB) \n");
             var produtosEletronicos = (from p in context.Produtos
                            where p.Categoria.Nome == "Eletrônicos"
                            select p).ToList();
            foreach(var produto in produtosEletronicos){
                Console.WriteLine($"{produto.Nome} - R${produto.Preco}");
            }

            //c) Buscar Produtos com Preço Acima de Um Valor (1.000):
            Console.WriteLine("\nC) \n");
            var produtosPreco1000 = (from p in context.Produtos
                            where p.Preco > 1000
                            select p).ToList();
            foreach(var produto in produtosPreco1000){
                Console.WriteLine($"{produto.Nome} - R${produto.Preco}");
            }

            //d) Ordenar Produtos por Preço:
            Console.WriteLine("\nD) \n");
            var produtosOrdenar = (from p in context.Produtos
                            orderby p.Preco ascending
                            select p).ToList();
            foreach(var produto in produtosOrdenar){
                Console.WriteLine($"{produto.Nome} - R${produto.Preco}");
            }
            //e) Agrupar Produtos por Categoria:
             Console.WriteLine("\nE) \n");
            var produtosAgrupar = (from p in context.Produtos
                            group p by p.Categoria.Nome into g
                            select new {Categoria = g.Key,
                            produtos =g.ToList()}).ToList();
            foreach(var grupo in produtosAgrupar){
                Console.WriteLine($"\nCategoria: {grupo.Categoria}\n");
                foreach(var produto in grupo.produtos)
                Console.WriteLine($"{produto.Nome} - R${produto.Preco}");
            }

            //f) Calcular a Receita Total de Vendas:
             Console.WriteLine("\nF) \n");
             var produtosreceitaTotal = (from v in context.Vendas 
             select v.Produto.Preco * v.Quantidade).Sum();
             Console.WriteLine($"Receita Total: R${produtosreceitaTotal}");

             //g) Listar todos os produtos junto com o nome de sua categoria (INNER JOIN).
             Console.WriteLine("\nG) \n");
                     var produtoscategoria = from p in context.Produtos
                                join c in context.Categorias on p.CategoriaId equals c.Id
                                select new 
                                {
                                    ProdutoNome = p.Nome,
                                    CategoriaNome = c.Nome,
                                    Preco = p.Preco
                                };
                        foreach (var item in produtoscategoria)
                        {
                            Console.WriteLine($"Produto: {item.ProdutoNome}, Categoria: {item.CategoriaNome}, Preço: R$ {item.Preco}");
                        }
            //h) Listar todos os produtos, incluindo aqueles que não possuem uma categoria associada (LEFT JOIN).
            Console.WriteLine("\nH) \n");
            var produtoscategoriaounao = from p in context.Produtos
                                join c in context.Categorias on p.CategoriaId equals c.Id into categoriaJoin
                                from c in categoriaJoin.DefaultIfEmpty()
                                select new 
                                {
                                    ProdutoNome = p.Nome,
                                    categoriaNome = c != null ? c.Nome : "Sem Categoria",
                                    Preco = p.Preco
                                };
            foreach (var item in produtoscategoriaounao)
                        {
                            Console.WriteLine($"Produto: {item.ProdutoNome}, Categoria: {item.categoriaNome}, Preço: R$ {item.Preco}");
                        }

            //i) Listar as vendas com informações do produto e da categoria associada a cada venda (INNER JOIN – 3 TABELAS).
            Console.WriteLine("\nI) \n");
            var produtosvendas = from p in context.Produtos
                                join c in context.Categorias on p.CategoriaId equals c.Id
                                join v in context.Vendas on p.Id equals v.ProdutoId
                                select new 
                                {
                                    VendaID = v.Id,
                                    ProdutoNome = p.Nome,
                                    CategoriaNome = c.Nome,
                                    TotalVenda = v.Quantidade * p.Preco
                                };
                        foreach (var item in produtosvendas)
                        {
                            Console.WriteLine($"VendaID: {item.VendaID}, Produto: {item.ProdutoNome}, Categoria: {item.CategoriaNome}, TotalVenda: R$ {item.TotalVenda}");
                        }

        }

    } 
} 
 
