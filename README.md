# Testes de Integração com xUnit

O objetivo deste projeto é realizar testes de integração de APIs utilizando o xUnit.
Serão cobertos cenários de :
* Chamadas com composição;
* Validação dos retornos;
* ContractTest vs Test Doubles.

Conhecimentos acerca de testes unitários e sobre xUnit não serão abordados.

## Chamadas com composição

O exemplo mais básico deste projeto é um teste com composição de duas APIS.

A primeira retorna uma imagem aleatório de gato, a segunda cria uma legenda para a foto;

* Api com foto aleatória de gatinhos: https://aws.random.cat/meow
* Api com a legenda: http://yerkee.com/api/fortune

O caráter não determinístico deste fluxo não é o ideal para o conceito de testes. O intuito aqui é apenas demonstrar a composição de duas chamadas.

``` c#
[Fact]
public async void CompositionTestAsync()
{
    HttpResponseMessage image = await "https://aws.random.cat/meow".GetAsync();
    HttpResponseMessage label = await "http://yerkee.com/api/fortune".GetAsync();

    Assert.True(image.IsSuccessStatusCode);
    Assert.True(label.IsSuccessStatusCode);
}
```

Para tornar o código mais fluente possível, utilizamos a biblioteca [Flurl](https://flurl.dev/) para realizar as chamadas HTTP
 Guilda Fractal

## Missão

A missão da Guilda Fractal é **zelar pela qualidade técnica dos projetos na dti**, **acelerando o aprendizado conceitual e prático dos crafters e parceiros**, servindo de **inspiração para a evolução profissional contínua**.

## OKRs

- **O1** = Reduzir leadtime de inicio dos projetos
  - **KR1** = 100% das tecnologias mapeadas com pessoas/squads de referência.
  - **KR2** = 100% dos squads com as tecnologias mapeadas.
- **O2** = Atuar na antifragilidade tecnológica da dti
  - **KR1** = Criar 2 POCs/mês de tecnologias que não estão presentes na dti, baseadas em problemas identificados nos checks.
  - **KR2** = 100% dos checks arquiteturais com pelo menos uma pessoa da Fractal presente.
  - **KR3** = Criar um novo Hour of Architect a cada 3 meses, e manter a execução de todos os já existentes.
## Validação dos retornos

O próximo exemplo trata da validação dos retornos. Usamos uma abordagem utilizando retorno dinâmico do c#, para ter uma abordagem mais fortemente tipada seria necessário criar um objeto que representasse o retorno da chamada. 

Considerando que o objetivo é acelerar a criação dos testes, a abordagem dinâmica é aconselhada, uma vez que precisamos preocupar apenas com os campos que serão testados, mas para realizar um teste de contrato completo, com todos os campos, a tipagem forte é aconselhada. 

``` c#
[Fact]
public async void ValidationTestAsync()
{
    dynamic result = await "https://openlibrary.org/api"
        .AppendPathSegment("books")
        .SetQueryParams(new { 
            bibkeys = "ISBN:0684153637", 
            jscmd = "data",
            format = "json"
        })
        .GetJsonAsync();
    var book = ((IDictionary<String, dynamic>)result)["ISBN:0684153637"];

    var title = book.title;
    var author = book.authors[0].name;

    Assert.Equal(title, "The old man and the sea");
    Assert.Equal(author, "Ernest Hemingway");
}
```

O teste acima utiliza uma [api de livros](https://openlibrary.org/) gratuita, para pesquisar um livro a partir do seu [ISBN](https://pt.wikipedia.org/wiki/International_Standard_Book_Number). A partir disso valida se o título do livro e seu autor são os esperados.