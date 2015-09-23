# Avatar

Aplicação feita em Unity que exibe um modelo 3D na tela e recebe, por meior do [plugin para Unity](https://github.com/lhsantos/unity_uos_plugin) do [Middleware uOS](https://github.com/UnBiquitous/uos_core), instruções para animar o modelo na tela. Tem como propósito auxiliar o jogador de jogos que usem movimento dos membros do jogador como input.

Internamente, ela oferece dois uOS Drivers, um para controlar um modelo de exemplo (AvatarGuide), e outro para controlar um modelo de feedback (AvatarFeedback).

## AvatarGuide

Oferece os seguintes serviços: 
- Animate: Faz o Avatar reproduzir uma animação dada. Recebe obrigatoriamente uma string contendo um JSON que especifica os passos de uma animação a ser realizada (mais detalhes abaixo)
- Pause: Pausa uma animação durante sua execução
- Resume: Continua uma animação pausada
- Reset: Reseta os membros do Avatar para a posição default.

#### Animação
(em breve)

## AvatarFeedback
(em breve)
