$(document).ready(function () {
    // Handle form submission via AJAX
    $("#leaveForm").submit(function (event) {
        event.preventDefault();
        var form = $(this);
        $.ajax({
            type: form.attr("method"),
            url: form.attr("action"),
            data: form.serialize(),
            success: function (response) {
                if (response.success) {
                    loadLeavesTable();
                } else {
                    alert(response.message);
                }
            }
        });
    });

    // Function to load the leaves table
    function loadLeavesTable() {
        $.get("@Url.Action('LeaveTable', 'LeaveManage')", function (data) {
            $("#leavesTableContainer").html(data);
            initializeTableEvents();
        });
    }

    // Function to initialize events for the table
    function initializeTableEvents() {
        $(document).on("click", ".increment", function () {
            var leaveId = $(this).data("id");
            var daysElement = $("#days-" + leaveId);
            var currentDays = parseInt(daysElement.text());
            daysElement.text(currentDays + 1);
            $("#saveBtn-" + leaveId).show();

        });

        $(document).on("click", ".decrement", function () {
            var leaveId = $(this).data("id");
            var daysElement = $("#days-" + leaveId);
            var currentDays = parseInt(daysElement.text());
            if (currentDays > 0) {
                daysElement.text(currentDays - 1);
                $("#saveBtn-" + leaveId).show();
            }
        });

        $(document).on("click", ".saveBtn", function () {
            var leaveId = $(this).attr("id").split("-")[1];
            var newDays = $("#days-" + leaveId).text();

            $.ajax({
                type: "post",
                url: 'UpdateLeaveDays',
                data: { id: leaveId, days: newDays },
                success: function (response) {
                    if (response.success) {
                        alert("Leave days updated successfully.");
                        $("#saveBtn-" + leaveId).hide();
                    } else {
                        alert(response.message);
                    }
                }
            });
        });
    }

    // Initialize events for the table on page load
    initializeTableEvents();
});
