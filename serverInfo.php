<?php
$server = "localhost";
$user = "uno";
$password = "1234";
$dbname = "unogame";

// 검색조건
$way = $_POST['way']; // 검색 방법 (0=uid, 1=nick)
$word = $_POST['word']; // user : uid

// database 연결
$conn = new mysqli($server, $user, $password, $dbname) or die("connect fail!".mysqli_error());
if(!$conn) die("connect fail!".mysql_error());

// $conn = mysql_connect($server,$user,$password) or die("connect fail!".mysqli_error());
// if(!$conn) die("connect fail!".mysql_error());


// User select
//쿼리 호출 방법
if($way == "0")     // 1 = uid 검색
    $sql = "SELECT * FROM USER WHERE uid = '".$word."'";
else if($way == "1")// 2 = 닉네임 검색
    $sql = "SELECT * FROM USER WHERE nick LIKE '%".$word."%'";


$user_result = mysqli_query($conn, $sql);
$row = mysqli_fetch_array($user_result);

echo $row['uid'];
echo "\n";
echo $row['nick'];

//while($user_row = mysql_fetch_array($user_result))
//{
//    echo "유저 uid : " + $user_result[0];
//} 

// mysql_free_result($user_result);//결과 지우기 
// mysql_close($conn);//db와 연결 끊기

?>
