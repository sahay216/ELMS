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

document.getElementById('checkInBtn').addEventListener('click', () => {
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
    $.post('/UserDashboard/CheckOut', function (data) {
        if (data.success) {
            document.getElementById('checkInBtn').disabled = false;
            document.getElementById('checkOutBtn').disabled = true;
            stopTimer();
        }
    });
});
