<?php
$dsn = "Connection string invoeren.";
$user = "BumbodiumAdmin";
$password = "NietBumboAdmin!";

// Create connection
$conn = odbc_connect($dsn, $user, $password);

if (!$conn) {
    die("Connection failed: " . odbc_errormsg());
}

$sql = "UPDATE Presence SET ClockOutDateTime = CURRENT_TIMESTAMP WHERE ClockOutDateTime = NULL";

$result = odbc_exec($conn, $sql);

if (!$result) {
    echo "Error updating record: " . odbc_errormsg();
} else {
    echo "Record updated successfully";
}

odbc_close($conn);
?>