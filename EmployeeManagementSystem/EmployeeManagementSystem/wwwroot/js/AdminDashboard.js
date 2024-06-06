const boxes = document.querySelectorAll('.box-card');
let draggedBox = null;


boxes.forEach(box => {
    box.addEventListener('dragstart', (e) => {
        draggedBox = box;
        setTimeout(() => {
            box.style.display = 'none';
        }, 0);
    });

    box.addEventListener('dragend', (e) => {
        setTimeout(() => {
            draggedBox.style.display = 'block';
            draggedBox = null;
        }, 0);
    });

    box.addEventListener('dragover', (e) => {
        e.preventDefault();
    });

    box.addEventListener('dragenter', (e) => {
        e.preventDefault();
        box.classList.add('drag-over');
    });

    box.addEventListener('dragleave', (e) => {
        box.classList.remove('drag-over');
    });

    box.addEventListener('drop', (e) => {
        box.classList.remove('drag-over');
        if (draggedBox !== box) {
            let temp = box.innerHTML;
            box.innerHTML = draggedBox.innerHTML;
            draggedBox.innerHTML = temp;
        }
    });
});
function incrementLeave(leaveId) {
    var input = document.getElementById(leaveId);
    input.value = parseInt(input.value) + 1;
}

function decrementLeave(leaveId) {
    var input = document.getElementById(leaveId);
    if (input.value > 0) {
    input.value = parseInt(input.value) - 1;
    }
}