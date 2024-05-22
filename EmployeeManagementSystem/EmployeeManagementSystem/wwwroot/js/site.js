<script>
    document.querySelector('.search-button').addEventListener('click', function(event) {
        event.preventDefault(); // Prevent default form submission
    var searchQuery = document.querySelector('.search-enter').value;
    // Redirect to the search action in your controller
    window.location.href = '@Url.Action("Search", "UserDashboard")' + '?query=' + encodeURIComponent(searchQuery);
    });

    document.querySelector('.search-enter').addEventListener('keypress', function(event) {
        if (event.key === 'Enter') {
        event.preventDefault();
    document.querySelector('.search-button').click();
        }
    });
</script>