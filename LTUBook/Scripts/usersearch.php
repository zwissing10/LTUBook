<?php
$servername = "Data Source=(LocalDb)\MSSQLLocalDB";
$username = "Search";
$password = "password";
$dbname = "aspnet-LTUBook-20190228033437";

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);
// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT Id, FirstName, LastName FROM AspNetUsers WHERE FullName LIKE '%" + $_GET["searchBox"] + "%'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    // output data of each row
    while($row = $result->fetch_assoc()) {
        echo "Id: " . $row["Id"]. " - Name: " . $row["FirstName"]. " " . $row["LastName"]. "<br>";
    }
} else {
    echo "0 results";
}
$conn->close();
?>