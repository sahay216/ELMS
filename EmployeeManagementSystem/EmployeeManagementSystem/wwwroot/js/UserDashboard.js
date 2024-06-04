const boxes = document.querySelectorAll('.box-card');
let draggedBox = null;
let timer;
let startTime;
let elapsedSeconds = 0;

function startTimer() {
    startTime = Date.now() - elapsedSeconds * 1000;
    timer = setInterval(() => {
        const currentTime = Date.now();
        elapsedSeconds = Math.floor((currentTime - startTime) / 1000);
        const hours = String(Math.floor(elapsedSeconds / 3600)).padStart(2, '0');
        const minutes = String(Math.floor((elapsedSeconds % 3600) / 60)).padStart(2, '0');
        const seconds = String(elapsedSeconds % 60).padStart(2, '0');
        document.getElementById('timer').textContent = `${hours}:${minutes}:${seconds}`;
    }, 1000);
}

function stopTimer() {
    clearInterval(timer);
}

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

document.querySelector('.search-button').addEventListener('click', function (event) {
    event.stopPropagation(); 
    event.preventDefault();
    var searchQuery = document.querySelector('.search-enter').value;
    var url = searchUrl + '/?query=' + encodeURIComponent(searchQuery.trim());

    fetch(url)
        .then(response => response.text())
        .then(data => {
            window.location.href = url;
        });
    alert(url);
});

document.querySelector('.search-enter').addEventListener('keypress', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
        document.querySelector('.search-button').click();
    }
});




document.getElementById('checkInBtn').addEventListener('click', () => {
    event.stopPropagation();
    event.preventDefault();
    $.post('/UserDashboard/CheckIn', function (data) {
        if (data.success) {
            document.getElementById('checkInBtn').disabled = true;
            document.getElementById('checkOutBtn').disabled = false;
            elapsedSeconds = 0;
            startTimer();
        }
    });
});

document.getElementById('checkOutBtn').addEventListener('click', () => {
    event.stopPropagation();
    event.preventDefault();
    $.post('/UserDashboard/CheckOut', function (data) {
        if (data.success) {
            document.getElementById('checkInBtn').disabled = false;
            document.getElementById('checkOutBtn').disabled = true;
            stopTimer();
        }
    });
});
