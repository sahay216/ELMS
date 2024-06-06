function validateLeaveApplication() {
    console.log("helo");
    const leaveTypeElement = document.getElementById('LeaveType');
    const selectedOption = leaveTypeElement.options[leaveTypeElement.selectedIndex];
    const availableBalance = parseInt(selectedOption.getAttribute('data-balance'));

    const leaveStartDate = document.getElementById('LeaveDateFrom').value;
    const leaveEndDate = document.getElementById('LeaveEnd').value;

    if (!leaveStartDate || !leaveEndDate) {
        alert('Please select both Start and End date.');
        return false;
    }

    const startDate = new Date(leaveStartDate);
    const endDate = new Date(leaveEndDate);

    if (endDate < startDate) {
        alert('End date cannot be before start date.');
        return false;
    }

    const timeDiff = Math.abs(endDate.getTime() - startDate.getTime());
    const diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24)) + 1;

    if (diffDays > availableBalance) {
        alert(`You are applying for ${diffDays} days of leave, but you only have ${availableBalance} of days available.`);
        return false;
    }
    return true;
}