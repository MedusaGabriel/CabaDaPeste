# 📘 Guia de Branches e Comandos Git - Projeto

## 🧠 Estrutura das Branches

- **main**  
  Versão final e estável do projeto.  
  ⚠️ **Atualmente só Gabriel** irá manipular essa branch.

- **development**  
  Central de desenvolvimento. Sempre terá a versão mais atual em progresso.  
  Todos os desenvolvedores devem **puxar e enviar alterações** a partir desta branch.

- **gabriel**  
  Branch do Gabriel.

- **brian**  
  Branch do Brian.

---

## 🚀 Regras IMPORTANTES!

1. **Nunca envie ou puxe alterações da `main`.**
2. Sempre trabalhe na **sua própria branch** (ex: `gabriel` ou `brian`).
3. Ao finalizar uma alteração, qualquer que seja, **envie para a branch `development`**.
4. A `development` será usada por todos para manter o projeto sincronizado.
5. Atualmente Gabriel será responsável por **atualizar a `main` a partir da `development`**, quando tudo estiver testado e funcionando.
6. **SEMPRE VERIFIQUE QUAL BRANCH VOCÊ ESTA** Você pode verificar atraves do git branch 


---

## 🛠️ Comandos Git

1. `git branch` - verifica ser ta na sua branch (exemplo: brian) 
2. `git pull origin development` - puxa as alterações da development 
3. `git add .` - adiciona as alterações 
4. `git commit -m "✨ mensagem das alterações ou titulo do que fez"` - git commit -m "✨ feat: descrição do que você fez"
5. `git push origin nome-da-sua-branch` (exemplo: git push origin brian)
6. **SEMPRE VERIFIQUE QUAL BRANCH VOCÊ ESTA** Você pode verificar atraves do git branch 

---

### 👉 1. Trocar de branch

`git checkout nome-da-branch`

Esse comando serve para que você mude a branch para outra exemplo: 

`git checkout gabriel`  **Troquei para branch gabriel**

### 👉 2. Puxar atualizações da development

`git pull origin development` 

Esse comando ele ira puxar as últimas alterações da development para sua branch local atual 

### 👉 3. Juntar sua branch com a development

`git merge `

Exemplo: 

`git merge gabriel` **Estou juntando as alterações que fiz na branch Gabriel com a development**

### 👉 4. Enviar as alterações para o GitHub

`git push origin development`**Envia as alterações da sua development local para a development do GitHub.** 
