# 📘 Guia de Branches e Comandos Git - Projeto

## 🧠 Estrutura das Branches

- **main**  
  Versão final e estável do projeto.  
  ⚠️ **Atualmente só Gabriel** irá manipular essa branch.

- **development**  
  Central de desenvolvimento. Sempre terá a versão mais atual em progresso.  
  Todos os desenvolvedores devem **puxar as alterações** a partir desta branch.
  Envios para essa branch e responsabilidade do **Gabriel**.

- **gabriel**  
  Branch do Gabriel.

- **brian**  
  Branch do Brian.

- **ryan**  
  Branch do Ryan.

-**Sera criado uma branch para o Anderson e Marcelo**
---

## 🚀 Regras IMPORTANTES!

1. **Nunca envie ou puxe alterações da `main`.**
2. Sempre trabalhe na **sua própria branch** (ex: `gabriel` ou `brian`).
3. Ao finalizar uma alteração, qualquer que seja, **envie para a sua branch** pois assim ira fica um registro de timeline do que você fez ou trabalho gerando um melhor controle.
4. A `development` será usada por todos para manter o projeto sincronizado.
5. Atualmente Gabriel será responsável por **atualizar a `main` a partir da `development`**, quando tudo estiver testado e funcionando.
6. **SEMPRE VERIFIQUE QUAL BRANCH VOCÊ ESTA** Você pode verificar atraves do git branch 


---

## 🛠️ Comandos Git

### 👉 1. Trocar de branch

`git checkout nome-da-branch`
Esse comando serve para que você mude a branch para outra exemplo: 
`git checkout gabriel`  **Troquei para branch gabriel**

### 👉 2. Puxar atualizações da development

`git pull origin development` 
Esse comando ele ira puxar as últimas alterações da development para sua branch local atual 

### 👉 3. Enviar as alterações para o GitHub

`git push origin nome-da-branch`**Envia as alterações da sua branch local para sua branch do github.** 
