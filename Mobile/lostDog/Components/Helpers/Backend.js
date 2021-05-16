
export const Backend_Switch =(FunName,Data,Token,Id)=>
{
    var URL= 'http://10.0.2.2:5000/'
    return new Promise((resolve,reject)=>
    {
        switch (FunName){
            case "Example":
                Example(Data,URL)
                .catch((x)=> reject(x))
                .then((x)=>{resolve(x)})
                break;
            case "login":
                login(Data,URL)
                .catch((x)=> {reject(x)})
                .then((x)=>  {resolve(x)})
                break;
            case "register":
                register(Data,URL)
                .catch((x)=> {reject(x)})
                .then((x)=>  {resolve(x)})
                break;
            case "registerNewDog":
                registerNewDog(Data,URL,Token)
                .catch((x)=> {reject(x)})
                .then((x)=>  {resolve(x)})
                break;
            case "getDogList":
                getDogList(null,URL,Token,Id)
                .catch((x)=> {reject(x)})
                .then((x)=>  {resolve(x)})
                break;
            case "getFilteredDogList":
                getFilteredDogList(Data,URL,Token,Id)
                .catch((x)=> {reject(x)})
                .then((x)=>  {resolve(x)})
                break;
            case "getShelterList":
                getShelterList(Data,URL,Token,Id)
                .catch((x)=> {reject(x)})
                .then((x)=>  {resolve(x)})
                break;
        }
    });
}

const Example = (data,url)=>{
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        console.log("Example promise Finished !!");
        resolve("Example promise finished successfully");
      }, 5000); // 5 sekund
    });
  }

const login = (data,url) =>
{
    console.log("LOGIN")
    return new Promise((resolve, reject) => {
        fetch(url+'login', {
            method: "POST",
            headers: {
              'Accept': '*/*',
            },
              body: data
            })
            .then(response=>{return response.json();})
            .then(responseData => {
                if (responseData.statusCode >= 400) {
                    console.log("ERROR")  
                    //console.log(responseData)  
                    reject(response.statusCode)
                    return null;
                }
                else
                {
                    resolve(responseData.data)
                    return (responseData.data);
                }
                })
            .catch((x)=>{
                reject(null)
                return null;
            })
      });
}

const register = (data,url) =>
{
    console.log("REGISTER")
    return new Promise((resolve, reject) => {
        fetch(url+'register', {
            method: "POST",
            headers: {
              'Accept': '*/*',
            },
              body: data
            })
            .then(response=>{return response.json();})
            .then(responseData => {
                if (responseData.statusCode >= 400) {
                    console.log("ERROR")  
                    //console.log(responseData)  
                    reject(response.statusCode)
                    return null;
                }
                else
                {
                    //console.log("THEN RESOLVE")
                    resolve(responseData)
                    return (responseData);
                }
                })
            .catch((x)=>{
                console.log("CATCH REJECT")
                reject(null)
                return null;
            })
      });
}

const registerNewDog = (data,url,Token) =>
{
    console.log("REGISTER_NEW_DOG")
    //console.log(data)
    return new Promise((resolve, reject) => {
        fetch(url+'lostdogs', {
            method: "POST",
            headers: {
              'Accept': '*/*',
              'Authorization': Token,
            },
              body: data
            })
            .then(response=>{
                //console.log(response)
                return response.json();
            })
            .then(responseData => {
                if (responseData.statusCode >= 400) {
                    console.log("ERROR")  
                    //console.log(responseData)  
                    reject(response.statusCode)
                    return null;
                }
                else
                {
                    //console.log("THEN RESOLVE")
                    resolve(responseData)
                    return (responseData);
                }
                })
            .catch((x)=>{
                console.log("CATCH REJECT")
                reject(null)
                return null;
            })
      });
}

const getDogList = (data,url,Token,id) =>
{
    console.log("getDogList")
    console.log("data: ")
    //console.log(data)
    return new Promise((resolve, reject) => {
        fetch(url+ 'lostdogs?ownerId='+id, {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
                'Accept': '*/*',
                'Authorization': Token,
            },
            })
            .then(response=>{
                //console.log(response)
                return response.json();
            })
            .then(responseData => {
                if (responseData.statusCode >= 400) {
                    console.log("ERROR")  
                    //console.log(responseData)  
                    reject(response.statusCode)
                    return null;
                }
                else
                {
                    //console.log("THEN RESOLVE")
                    resolve(responseData.data)
                    return (responseData.data);
                }
                })
            .catch((x)=>{
                console.log("CATCH REJECT")
                reject(null)
                return null;
            })
      });
}

const getFilteredDogList = (data,url,Token,id) =>
{
    console.log("getFilteredDogList")
    console.log("data: " + data)
    newUrl=url+'lostdogs?'

    if(data.breed!="")
    {
        newUrl+='filter.breed='+data.breed+'&'
    }
    if(data.ageFrom!="")
    {
        newUrl+='filter.ageFrom='+data.ageFrom+'&'
    }
    if(data.ageTo!="")
    {
        newUrl+='filter.ageTo='+data.ageTo+'&'
    }
    if(data.size!="")
    {
        newUrl+='filter.size='+data.size+'&'
    }
    if(data.color!="")
    {
        newUrl+='filter.color='+data.color+'&'
    }
    if(data.name!="")
    {
        newUrl+='filter.name='+data.name+'&'
    }
    if(data.locationCity!="")
    {
        newUrl+='filter.location.city='+data.locationCity+'&'
    }
    if(data.locationDistrict!="")
    {
        newUrl+='filter.location.district='+data.locationDistrict+'&'
    }
    if(data.dateLostBefore!="")
    {
        newUrl+='filter.dateLostBefore='+data.dateLostBefore+'&'
    }
    if(data.dateLostAfter!="")
    {
        newUrl+='filter.dateLostAfter='+data.dateLostAfter+'&'
    }
    //sort=color,DESC&page=1&size=20'
    console.log("newUrl")
    console.log(newUrl)
    return new Promise((resolve, reject) => {
        fetch(newUrl, {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
                'Accept': '*/*',
                'Authorization': Token,
            },
            })
            .then(response=>{
                //console.log(response)
                return response.json();
            })
            .then(responseData => {
                if (responseData.statusCode >= 400) {
                    console.log("ERROR")  
                    //console.log(responseData)  
                    reject(response.statusCode)
                    return null;
                }
                else
                {
                    //console.log("THEN RESOLVE")
                    resolve(responseData.data)
                    return (responseData.data);
                }
                })
            .catch((x)=>{
                console.log("CATCH REJECT")
                reject(null)
                return null;
            })
      });
}

const getShelterList = (data,url,Token,id) =>
{
    console.log("getDogList")
    console.log("data: ")
    //console.log(data)
    return new Promise((resolve, reject) => {
        fetch(url+ 'shelters', {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
                'Accept': '*/*',
                'Authorization': Token,
            },
            })
            .then(response=>{
                //console.log(response)
                return response.json();
            })
            .then(responseData => {
                if (responseData.statusCode >= 400) {
                    console.log("ERROR")  
                    //console.log(responseData)  
                    reject(response.statusCode)
                    return null;
                }
                else
                {
                    //console.log("THEN RESOLVE")
                    resolve(responseData.data)
                    return (responseData.data);
                }
                })
            .catch((x)=>{
                console.log("CATCH REJECT")
                reject(null)
                return null;
            })
      });
}