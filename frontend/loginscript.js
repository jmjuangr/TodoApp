document.addEventListener("DOMContentLoaded", () => {
    const loginForm = document.getElementById("loginForm");

    if (loginForm) {
        loginForm.addEventListener("submit", async (event) => {
            event.preventDefault();

            const username = document.getElementById("username").value.trim();
            const password = document.getElementById("password").value.trim();
            const loginError = document.getElementById("loginError");

            if (!username || !password) {
                loginError.textContent = "Usuario y contraseña requeridos";
                return;
            }

            try {
               
                const response = await fetch("http://backend:8080/api/usuarios/login", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ nombreUsuario: username, password }) 
                });

                if (!response.ok) throw new Error("Usuario o contraseña incorrectos");

                const userData = await response.json();
                localStorage.setItem("user", JSON.stringify(userData)); 
                window.location.href = "web.html"; 
                localStorage.setItem("lastUser", userData.nombreUsuario);
                
            } catch (error) {
                loginError.textContent = "Usuario o contraseña incorrectos";
            }
        });
    }
});
