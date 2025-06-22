# Unity-Minesweeper

Desenvolvimento do jogo Minesweeper (Campo Minado) em ambiente Unity e C#.<br>
Focando em implementar melhorias QOF (quality-of-life) e novas funcionalidades.
- [build atual](https://drive.google.com/drive/folders/19gcA9AAVsJlNWkiLAWmwGsL1JUTuml5G?usp=sharing)
<hr>

<h3>Comandos:</h3>

><b>Clique esquerdo (LMB)</b>: Desbloqueia campo alvo.<br>

><b>Clique do meio (MMB)</b>: Desbloqueia todos os campos adjacentes (e diagonais) ao campo alvo.<br>
>(Só é possível ser acionado quando houver no mínimo X minas marcadas ao redor do campo alvo sendo
>X o número de minas ao redor do campo)

><b>Clique direito (RMB)</b>: Marca mina (bandeira).<br>

><b>R</b>: Reinicia o jogo.<br>

<h3>Implementado:</h3>

- Grid do jogo, assets e mapeamento das tiles;
- Geração aleatória das minas;
- Interface gráfica provisória;
- Revelar;
- Espalhar (recursiva);
- Marcar;
- Explodir/Perder (e resposta gráfica);
- Revelar ao redor;
- Prevenir atingir mina no primeiro clique do jogo (gerar minas a partir do 1º clique);
- Geração de campo inicial (quadrado seguro ao redor do 1º clique);
- Exibição de flags verdes ao perder o jogo, sinalizando onde houve o erro do jogador
(flag verdes: área em que foi colocada bandeira, mas não há mina);
- Tratamento de Input.

<h3>Falta implementar:</h3>

- Algumas melhorias qof?;
- Impl. melhor interface gráfica, outras stats e msg de fim de jogo;
<hr>
