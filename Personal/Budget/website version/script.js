let items = JSON.parse(localStorage.getItem("budgetItems")) || [];
let savingsGoal = parseFloat(localStorage.getItem("savingsGoal")) || 0;

function setSavingsGoal() {
    const goalInput = document.getElementById("savingsGoalInput").value;
    if (goalInput !== "") {
        savingsGoal = parseFloat(goalInput);
        localStorage.setItem("savingsGoal", savingsGoal);
        updateSummary();
    }
}

function addItem() {
    const name = document.getElementById("name").value;
    const amount = parseFloat(document.getElementById("amount").value);
    const type = document.getElementById("type").value;
    const categoryOrSource = document.getElementById("categoryOrSource").value;

    if (name && !isNaN(amount) && categoryOrSource) {
        items.push({ name, amount, type, categoryOrSource });
        localStorage.setItem("budgetItems", JSON.stringify(items));
        updateItemsList();
        updateSummary();

        // Clear inputs
        document.getElementById("name").value = "";
        document.getElementById("amount").value = "";
        document.getElementById("categoryOrSource").value = "";
    }
}

function resetBudget() {
    if (confirm("Are you sure you want to reset the budget?")) {
        items = [];
        savingsGoal = 0;
        localStorage.removeItem("budgetItems");
        localStorage.removeItem("savingsGoal");
        updateItemsList();
        updateSummary();
    }
}

function updateItemsList() {
    const itemsList = document.getElementById("itemsList");
    itemsList.innerHTML = "";
    items.forEach(item => {
        const li = document.createElement("li");
        li.textContent = `${item.type === "income" ? "Income" : "Expense"} - ${item.name}: $${item.amount.toFixed(2)} (${item.categoryOrSource})`;
        itemsList.appendChild(li);
    });
}

function updateSummary() {
    const totalIncome = items.filter(i => i.type === "income").reduce((sum, i) => sum + i.amount, 0);
    const totalExpenses = items.filter(i => i.type === "expense").reduce((sum, i) => sum + i.amount, 0);
    const balance = totalIncome - totalExpenses;

    document.getElementById("balance").textContent = `Current Balance: $${balance.toFixed(2)}`;
    document.getElementById("savingsGoal").textContent = `Savings Goal: $${savingsGoal.toFixed(2)}`;
}

updateItemsList();
updateSummary();
