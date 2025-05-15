export function initializeDropdown() {
    const btn = document.getElementById("langSelectDropdownBtn");
    const menu = document.getElementById("langSelectDropdownMenu");
    const selectedValuesDisplay = document.getElementById("selectedValues");

    if (!btn || !menu || !selectedValuesDisplay) return false;

    // Remove existing event listeners if any
    const oldBtn = btn.cloneNode(true);
    btn.parentNode.replaceChild(oldBtn, btn);

    // Re-assign variables after DOM replacement
    const newBtn = document.getElementById("langSelectDropdownBtn");

    // DROPDOWN ANIMATIONS
    newBtn.addEventListener("click", () => {
        menu.classList.toggle("show");
    });

    // Close on click/touch outside & escape key
    document.addEventListener("click", (e) => {
        if (!newBtn.contains(e.target) && !menu.contains(e.target)) {
            menu.classList.remove("show");
        }
    });

    document.addEventListener("keydown", (e) => {
        if (e.key === "Escape") {
            menu.classList.remove("show");
        }
    });

    // CHECKBOX VALUE DISPLAY OUTSIDE
    const checkboxes = menu.querySelectorAll('input[type="checkbox"]');
    checkboxes.forEach(checkbox => {
        checkbox.addEventListener("change", updateSelectedValues);
    });

    function updateSelectedValues() {
        const selected = [];
        checkboxes.forEach(checkbox => {
            if (checkbox.checked) {
                const labelText = checkbox.parentElement.textContent.trim();
                selected.push(labelText);
            }
        });

        if (selected.length === 0) {
            selectedValuesDisplay.textContent = "Select one or more languages";
            selectedValuesDisplay.classList.add("empty-state");
        } else {
            selectedValuesDisplay.textContent = selected.join(", ");
            selectedValuesDisplay.classList.remove("empty-state");
        }
    }

    // Initialize selected values
    updateSelectedValues();

    return true;
}