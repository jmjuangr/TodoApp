document.addEventListener("DOMContentLoaded", () => {
    const API_URL = "http://localhost:5175/api/Tareas";
    const taskForm = document.getElementById("taskForm");
    const taskTitle = document.getElementById("taskTitle");
    const taskDescription = document.getElementById("taskDescription");
    const taskDate = document.getElementById("taskDate");
    const taskPriority = document.getElementById("taskPriority");
    const highPriority = document.getElementById("highPriority");
    const mediumPriority = document.getElementById("mediumPriority");
    const lowPriority = document.getElementById("lowPriority");

    const lastUserName = document.getElementById("lastUserName");

    // 🔹 Recuperar el último usuario que inició sesión
    const lastUser = localStorage.getItem("lastUser");

    if (lastUser && lastUserName) {
        lastUserName.textContent = `Hola  ${lastUser}`;
    }

    async function loadTasks() {
        try {
            const response = await fetch(API_URL);
            if (!response.ok) throw new Error("Error al cargar las tareas");
            const tasks = await response.json();
            tasks.forEach(renderTask);
        } catch (error) {
            console.error(error);
        }
    }

    function renderTask(task) {
        const taskCard = document.createElement("div");
        taskCard.classList.add("task-card");
        taskCard.setAttribute("draggable", true);
        taskCard.dataset.id = task.tareaId;
    
        // 🔹 Crear contenido de la tarea
        taskCard.innerHTML = `
            <strong class="task-title">${task.titulo}</strong>
            <div class="task-actions">
                <button class="edit-task">Editar</button>
                <button class="delete-task">Eliminar</button>
            </div>
            <div class="task-details hidden">
                <p><strong>Descripción:</strong> ${task.descripcion}</p>
                <p><strong>Fecha límite:</strong> ${task.fecha}</p>
            </div>
        `;
    
        // 🔹 Guardar datos completos en dragstart como JSON
        taskCard.addEventListener("dragstart", (event) => {
            const taskData = JSON.stringify(task); // Convertimos la tarea en JSON
            event.dataTransfer.setData("taskData", taskData);
            taskCard.classList.add("dragging");
        });
    
        taskCard.addEventListener("dragend", () => {
            taskCard.classList.remove("dragging");
        });
    
        // 🔹 Agregar eventos de edición y eliminación
        taskCard.querySelector(".delete-task").addEventListener("click", async (event) => {
            event.stopPropagation();
            await deleteTask(task.tareaId);
            taskCard.remove();
        });
    
        taskCard.querySelector(".edit-task").addEventListener("click", async (event) => {
            event.stopPropagation();
            editTask(task);
        });
    
        assignTaskToColumn(taskCard, task.prioridad);
    }
    
    // 🔹 Manejo de drag & drop en las columnas
    document.querySelectorAll(".task-column").forEach((column) => {
        column.addEventListener("dragover", (event) => {
            event.preventDefault(); // Permitir soltar
        });
    
        column.addEventListener("drop", async (event) => {
            event.preventDefault();
        
            const taskData = event.dataTransfer.getData("taskData");
            if (!taskData) return;
        
            const task = JSON.parse(taskData);
            const taskCard = document.querySelector(`[data-id='${task.tareaId}']`);
        
            if (taskCard) {
                column.appendChild(taskCard);
        
                const nuevaPrioridad = column.dataset.priority;
                task.prioridad = nuevaPrioridad;
        
                // 🔹 Actualizar la interfaz de usuario
                taskCard.querySelector(".task-title").textContent = task.titulo;
                taskCard.querySelector(".task-details p:nth-child(1)").innerHTML = `<strong>Descripción:</strong> ${task.descripcion}`;
                taskCard.querySelector(".task-details p:nth-child(2)").innerHTML = `<strong>Fecha límite:</strong> ${task.fecha}`;
        
                await updateTask(task);
            }
        });
    });
    
    // 🔹 Función para actualizar la prioridad en la API
    async function updateTask(task) {
        try {
            const response = await fetch(`${API_URL}/${task.tareaId}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(task) // Enviamos toda la tarea actualizada
            });
    
            if (!response.ok) throw new Error("Error al actualizar la tarea");
            console.log(`✅ Tarea ${task.tareaId} actualizada correctamente`);
        } catch (error) {
            console.error("⚠️ Error al actualizar la tarea:", error);
        }
    }
    taskForm.addEventListener("submit", async (event) => {
        event.preventDefault();
        const fechaValue = taskDate.value ? `${taskDate.value}T00:00:00` : new Date().toISOString();
      


        const newTask = {
    
            titulo: taskTitle.value.trim(),
            descripcion: taskDescription.value.trim(),
            fecha: fechaValue,
            estado: false,
            prioridad: taskPriority.value.trim(),
        };

        console.log("Enviando tarea:", JSON.stringify(newTask, null, 2));


        try {
            const response = await fetch(API_URL, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(newTask)
            });
            if (!response.ok) throw new Error("Error al crear la tarea");
            const createdTask = await response.json();
            renderTask(createdTask);
            taskForm.reset();
        } catch (error) {
            console.error(error);
        }
    });

    async function deleteTask(id) {
        try {
            await fetch(`${API_URL}/${id}`, { method: "DELETE" });
        } catch (error) {
            console.error("Error al eliminar la tarea", error);
        }
    }

    async function editTask(task) {
        const newTitle = prompt("Editar título", task.titulo);
        const newDescription = prompt("Editar descripción", task.descripcion);
        const newDate = prompt("Editar fecha límite", task.fecha);
        const newPriority =prompt("Editar prioridad", task.prioridad);

        if (!newTitle || !newDescription || !newDate) return;

        const updatedTask = { ...task, titulo: newTitle, descripcion: newDescription, fecha: newDate, prioridad: newPriority };

        try {
            const response = await fetch(`${API_URL}/${task.tareaId}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(updatedTask)
            });
            if (!response.ok) throw new Error("Error al actualizar la tarea");
            document.querySelector(`[data-id='${task.tareaId}'] .task-title`).textContent = newTitle;
            document.querySelector(`[data-id='${task.tareaId}'] .task-details p:nth-child(1)`).innerHTML = `<strong>Descripción:</strong> ${newDescription}`;
            document.querySelector(`[data-id='${task.tareaId}'] .task-details p:nth-child(2)`).innerHTML = `<strong>Fecha límite:</strong> ${newDate}`;
            document.querySelector(`[data-id='${task.tareaId}'] .task-details p:nth-child(3)`).innerHTML = `<strong>Prioridad:</strong> ${newPriority}`;
        } catch (error) {
            console.error(error);
        }
    }

    function assignTaskToColumn(taskCard, prioridad) {
        if (prioridad === "Alta") {
            highPriority.appendChild(taskCard);
        } else if (prioridad === "Media") {
            mediumPriority.appendChild(taskCard);
        } else {
            lowPriority.appendChild(taskCard);
        }
    }

    loadTasks();
});
