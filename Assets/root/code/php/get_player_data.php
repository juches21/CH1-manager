<?php
include 'db_connect.php';

$query = "SELECT * FROM piloto";
$result = $conn->query($query);

$players = array();
while($row = $result->fetch_assoc()) {
    $players[] = $row;
}
echo json_encode($players);
?>
