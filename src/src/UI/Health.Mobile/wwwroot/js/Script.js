
let map;
let currentLong;
let currentLat;
let marker;
let markOptions;
export function mapFunction(lat, long, latEnd, longEnd)
{
    if(navigator.geolocation)
    {
        let options = {
            zoom: 14,
            center:[lat,long],
        };
        map =L.map(document.getElementById("map"), options).setView([9.005401, 38.763611], 14);
        let layer= L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png');
        layer.addTo(map)
        // let layer= tt.map({
        //     key: "FZcHRcYacU6ZwuViMdIkU6abppnRWeir",
        //     container: document.getElementById("map"),
        //     center: [lat,long],
        // });
        let customIcon= L.icon({iconUrl:"_content/Health.Mobile/images/YourLocation.png", iconSize:[50,50]});
        markOptions = {
            title:"My Locations",
            draggable:true,
            icon:customIcon
        }
        //     let waypoints= [
        //         [lat, long],
        //        [latEnd, longEnd]
        //      ]
        // let patterns= [
        //      // defines a pattern of 10px-wide dashes, repeated every 20px on the line
        //     {color: 'green',weight: 5,
        //         opacity: 0.7,
        //         dashArray: '4,12,20,12',
        //         lineJoin: 'miter',
        //     }
        //  ]
        //         let polyLine = L.polyline(waypoints, patterns ).addTo(map);
        L.Routing.control({
            waypoints: [
                L.latLng(lat, long),
                L.latLng(latEnd, longEnd)
            ],
            serviceUrl:"http://127.0.0.1:5000/route/v1",
            showAlternatives: true,
            altLineOptions:{
                styles:[
                    {color:"green", opacity:0.1, weight:9},
                    {color:"white", opacity:0.8, weight:6},
                    {color:"yellow", opacity:0.5, weight:2}
                ]
            }

        }).addTo(map);
        marker=   L.marker([lat,long], markOptions).addTo(map);
    }
}
// export function tomTomMapFunction(lat, long, latEnd, longEnd)
// {
//     if(navigator.geolocation)
//     {
//          map = tt.map({
//             key: "FZcHRcYacU6ZwuViMdIkU6abppnRWeir",
//             container: document.getElementById("map"),
//             
//         })
//          marker = new tt.Marker()
//             .setLngLat([lat, long])
//             .addTo(map);
//         let origin = [
//             { point: { latitude: lat, longitude: long } }
//         ];
//         let destinations = [
//             { point: { latitude: lat, longitude: long } },
//             { point: { latitude: latEnd, longitude: longEnd } }
//         ];
//         // let options = {
//         //     zoom: 14,
//         //     center:[lat,long],
//         // };
//         // map =tt.map(document.getElementById("map"), options).setView([9.005401, 38.763611], 14);
//         // let layer= L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png');
//         // layer.addTo(map)
//         // let customIcon= L.icon({iconUrl:"_content/Health.Mobile/images/YourLocation.png", iconSize:[50,50]});
//         // markOptions = {
//         //     title:"My Locations",
//         //     draggable:true,
//         //     icon:customIcon
//         // }
//         // //     let waypoints= [
//         // //         [lat, long],
//         // //        [latEnd, longEnd]
//         // //      ]
//         // // let patterns= [
//         // //      // defines a pattern of 10px-wide dashes, repeated every 20px on the line
//         // //     {color: 'green',weight: 5,
//         // //         opacity: 0.7,
//         // //         dashArray: '4,12,20,12',
//         // //         lineJoin: 'miter',
//         // //     }
//         // //  ]
//         // //         let polyLine = L.polyline(waypoints, patterns ).addTo(map);
//         // L.Routing.control({
//         //     waypoints: [
//         //         L.latLng(lat, long),
//         //         L.latLng(latEnd, longEnd)
//         //     ],
//         //     serviceUrl:"http://127.0.0.1:5000/route/v1",
//         //     showAlternatives: true,
//         //     altLineOptions:{
//         //         styles:[
//         //             {color:"green", opacity:0.1, weight:9},
//         //             {color:"white", opacity:0.8, weight:6},
//         //             {color:"yellow", opacity:0.5, weight:2}
//         //         ]
//         //     }
//         //
//         // }).addTo(map);
//         // marker=   L.marker([lat,long], markOptions).addTo(map);
//     }
// }
export function currentLocation()
{
    if(navigator.geolocation)
    {
        navigator.geolocation.getCurrentPosition(success, error, options);
        if(currentLat!=null && currentLong!=null)
        {
            if(marker)
            {
                map.removeLayer(marker);
                marker=   L.marker([currentLat,currentLong], markOptions).addTo(map);
            }
        }
    }

}
const options = {
    enableHighAccuracy: true,
    timeout: 5000,
    maximumAge: 0
};

function success(pos) {
    const crd = pos.coords;
    currentLat=crd.latitude
    currentLong=crd.longitude
}

function error(err) {
    switch (err.code)
    {
        case err.PERMISSION_DENIED :
        {
            alert("User Denied the request for location")
        }
            break
        case err.POSITION_UNAVAILABLE :
        {
            alert(" Location information is unavailable please update your browser")
        }
            break
        case err.TIMEOUT :
        {
            alert(" The request for location is timed out")
        }
            break
        case err.UNKNOWN_ERR :
        {
            alert(err.message)
        }
    }

}




export function automatic()
{
    let counter =1;
    setInterval(function ()
    {
        document.getElementById('radio'+counter).checked=true;
        counter++;
        if(counter>4)
        {
            counter=1;
        }
    }, 8000)
}
export  function  load(url) {
    window.open(url,"_blank")
}
export function copy(text){
    navigator.clipboard.writeText(text)
}