# Unity-Minesweeper

Desenvolvimento do jogo Minesweeper (Campo Minado) em ambiente Unity e C#.<br>
Focando em implementar melhorias QOF (quality-of-life) e novas funcionalidades.
- [v0.1](https://drive.google.com/drive/folders/19gcA9AAVsJlNWkiLAWmwGsL1JUTuml5G?usp=sharing)
<hr>

<h3>Comandos:</h3>

>Clique esquerdo (LMB): Desbloqueia campo alvo.<br>
>Clique do meio (MMB): Desbloqueia todos os campos adjacentes (e diagonais) ao campo alvo.<br>
>Clique direito (RMB): Marca mina (bandeira).<br>
>R: Reinicia o jogo.<br>

<h3>Implementado:</h3>

- Grid do jogo, assets e mapeamento das tiles;
- Geração aleatória das minas;
- Interface gráfica provisória;
- Revelar;
- Espalhar (recursiva);
- Marcar;
- Explodir/Perder (e resposta gráfica);
- Revelar ao redor.
- Tratamento de Input

<h3>Falta implementar:</h3>

- Algumas melhorias qof;
- Prevenir atingir mina no primeiro clique do jogo (gerar campo minado a partir do clique);
- Pensar em qual abordagem acima será usada pra garantir uma boa área de jogo no 1º clique;
- Mostrar apenas minas não marcadas (flagged) quando atinge uma mina e perde;
- Impl. melhor interface gráfica, outras stats e msg de fim de jogo;
<hr>
