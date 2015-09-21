<?php

$trackerDb = new PDO("sqlsrv:Server=.\SQLEXPRESS,1433;Database=tracker", 'tracker' , '123456taina');
$dataStorageDb = new PDO("sqlsrv:Server=.\SQLEXPRESS,1433;Database=DataStorage", 'tracker' , '123456taina');

$jobsQuery = $trackerDb->query("SELECT
                        j.Id as jobId,
                        c.name as categoryName,
                        j.Letter,
                        j.CrawledPages,
                        j.PagesCount,
                        u.UserName,
                        j.LastAction
                    FROM Jobs j
                    JOIN AspNetUsers u on u.Id = j.UserId
                    JOIN Categories c on c.Id = j.CategoryId
                    ORDER BY j.LastAction");
$lastAppsQuery = $dataStorageDb->query("SELECT TOP 10 trackId, trackName, trackViewUrl, formattedPrice FROM Apps ORDER BY Id");
if(isset($_GET['countApps'])) {
    $uniqueAppsQuery = $dataStorageDb->query("SELECT count(distinct trackId) as [count] FROM Apps");
    $uniqueAppsNum = $uniqueAppsQuery->fetchAll(PDO::FETCH_ASSOC);
    $uniqueAppsNum = $uniqueAppsNum[0]['count'];
}
?>
<div class="wrapper">
    <h3>Active jobs</h3>
    <table>
        <thead>
        <tr>
            <td>Job ID</td>
            <td>Category name</td>
            <td>Letter</td>
            <td>Crawled Pages</td>
            <td>Pages count</td>
            <td>Username</td>
            <td>Last action</td>
        </tr>
        </thead>
        <tbody>
        <?php
        while($job = $jobsQuery->fetch(PDO::FETCH_ASSOC)){
            echo '<tr>
                <td>' . $job["jobId"] .'</td>
                <td>' . $job["categoryName"] .'</td>
                <td>' . $job["Letter"] .'</td>
                <td>' . $job["CrawledPages"] .'</td>
                <td>' . $job["PagesCount"] .'</td>
                <td>' . $job["UserName"] .'</td>
                <td>' . $job["LastAction"] .'</td>
            </tr    >';
        }
        ?>
        </tbody>
    </table>   <br/>
    <h3>Latest 10 apps</h3>
    <table>
        <thead>
        <tr>
            <td>App ID</td>
            <td>Name</td>
            <td>Price</td>
        </tr>
        </thead>
        <tbody>
        <?php
        while($app = $lastAppsQuery->fetch(PDO::FETCH_ASSOC)){
            echo '<tr>
                     <td>' . $app['trackId'] . '</td>
                     <td><a href="' . $app['trackViewUrl'] . '">' . $app['trackName'] . '</a></td>
                     <td>' . $app['formattedPrice'] . '</td>
                </tr>';
        }
        ?>
        </tbody>
    </table>
    <p id="total"><b>Total unique apps: </b> <?= isset($_GET['countApps']) ? $uniqueAppsNum . ' <a href="stats">Hide</a>'
            : '<a href="?countApps">Show</a>'?></p>
</div>