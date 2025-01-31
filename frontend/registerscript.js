document.addEventListener("DOMContentLoaded", () => {
    const registerForm = document.getElementById("registerForm");

    if (registerForm) {
        registerForm.addEventListener("submit", async (event) => {
            event.preventDefault();

            const username = document.getElementById("username").value.trim();
            const password = document.getElementById("password").value.trim();
            const confirmPassword = document.getElementById("confirmPassword").value.trim();
            const registerError = document.getElementById("registerError");

            if (!username || !password || !confirmPassword) {
                registerError.textContent = "Todos los campos son obligatorios.";
                return;
            }

            if (password !== confirmPassword) {
                registerError.textContent = "Las contraseñas no coinciden.";
                return;
            }

            try {
                const response = await fetch("http://localhost:5175/api/usuarios/register", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ nombreUsuario: username, password })
                });

                if (!response.ok) {
                    throw new Error("El nombre de usuario ya está en uso.");
                }

                alert("Registro exitoso. Ahora puedes iniciar sesión.");
                window.location.href = "login.html"; // Redirige al login

            } catch (error) {
                registerError.textContent = error.message;
            }
        });
    }
});
