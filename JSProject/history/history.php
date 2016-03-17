<?php
    $data = json_decode(file_get_contents("php://input"));
    header("Content-Type: application/json; charset=utf-8");
    echo ("[{"age" : 24, "sex" : "boy", "name" : "huangxueming"},{"age" : 26, "sex" : "boy", "name" : "huangxueming2"}]");
?>