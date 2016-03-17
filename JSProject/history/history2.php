<?php
    $data = json_decode(file_get_contents("php://input"));
    header("Content-Type: application/json; charset=utf-8");
    echo ("[{"age" : 65, "sex" : "boy2", "name" : "huangxueming2"},{"age" : 26, "sex" : "boy", "name" : "huangxueming2"}]");
?>