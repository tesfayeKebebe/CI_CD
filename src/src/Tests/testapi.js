import http from 'k6/http';
import { sleep } from 'k6';
export const options={
   stages:[
    {duration:'30s', target:20},
    {duration:'1m30s', target:10},
    {duration:'20s', target:0}
   ]
}
export default function(){
    http.get('https://localhost:7241/api/LabTest/TestCategory');
    sleep(1);
}