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
                // 🔹 Cambiar el método de "GET" a "POST"
                const response = await fetch("http://localhost:5175/api/usuarios/login", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ nombreUsuario: username, password }) // Enviar JSON válido
                });

                if (!response.ok) throw new Error("Usuario o contraseña incorrectos");

                const userData = await response.json();
                localStorage.setItem("user", JSON.stringify(userData)); // Guardar sesión en localStorage
                window.location.href = "web.html"; // 🔹 Redirigir a la página correcta
                localStorage.setItem("lastUser", userData.nombreUsuario);
                
            } catch (error) {
                loginError.textContent = "Usuario o contraseña incorrectos";
            }
        });
    }
});
