<?php
    $data = json_decode(file_get_contents("php://input"));
    header("Content-Type: application/json; charset=utf-8");
    echo ("[{"age" : 244, "sex" : "boy4", "name" : "huangxueming4"},{"age" : 264, "sex" : "boy4", "name" : "huangxueming4"}]");
?>