<!doctype html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Project X</title>
    <link rel="stylesheet" href="/ProjectX/css/style.css"/>
</head>
<body>
<ul id="menu">
    <li><a href="/ProjectX/stats">stats</a></li>
    <li><a href="/ProjectX/latest">latest</a></li>
    <li><a target="_blank" href="https://github.com/mar0der/x">github</a></li>
</ul>
<?php
    print_r($_GET);
    if(isset($_GET['url'])){
        if($_GET['url'] == 'stats'){
            include 'jobInfo.php';
        }elseif($_GET['url'] == 'latest'){
            include 'latest.php';
        }
    }

?>
</body>
</html>