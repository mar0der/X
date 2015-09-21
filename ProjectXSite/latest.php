<?php

$dir = scandir(realpath('./latestVersion'));
$files = array_slice($dir, 2);
$files = array_reverse($files);


?>
<div id="versionWrapper">
    <?php
    echo '<ul id="versionList">';
    foreach($files as $file) {
        if(preg_match('/\.zip$/', $file)){
            echo '<li><a href="/ProjectX/latestVersion/' . $file . '"> ' . $file .  '</a></li>';
        }
    }
    echo '</ul>';
    ?>
</div>
