#!/bin/sh

proc_name="./server"

proc_num()
{  
    num=`ps -ef | grep $proc_name | grep -v grep | wc -l`  
    return $num  
} 

log_num=1

while true

do

proc_num
number=$?

echo "process "$proc_name" number = $number"

if [ $number -eq 0 ]

then

nohup $proc_name ./log/log$log_num &

echo "restart process $proc_name, store log to "./log/log$log_num""
log_num=$(($log_num+1));

else

echo "process $proc_name already started!"

fi

sleep 10

done
