import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity, Image,ScrollView,SafeAreaView } from 'react-native';
import * as ImagePicker from 'expo-image-picker';
import addDogIcon from '../Assets/addDogIcon.png';
import camera from '../Assets/camera.png';
import gallery from '../Assets/gallery.png';
import AppIcon from '../Assets/AppIcon.png'
import newDog from '../Assets/newdog.png'


const {width, height} = Dimensions.get("screen")


export default class RegisterNewDog extends React.Component {
  state={
    image: null,
    breed: "dogdog",
    age: "5",
    size: "Large",
    color: "Orange",
    specialMark: "tatto on the neck",
    name: "Cat",
    hairLength: "Long",
    tailLength: "None",
    earsType: "Short",
    behaviour1: "Angry",
    behaviour2: "Sad",
    LocationCity: "Biała",
    LocationDistinct: "Small",
    dateLost: "2021-03-20",
  }

  pickImage = async () => {
    let result = await ImagePicker.launchImageLibraryAsync({
      mediaTypes: ImagePicker.MediaTypeOptions.All,
      allowsEditing: false,
      aspect: [4, 3],
      quality: 1,
    });

    if (!result.cancelled) {
      console.log(result);
      this.setState({image: result.uri});
    }
  };
  makeImage = async () => {
    let result = await ImagePicker.launchCameraAsync({
      mediaTypes: ImagePicker.MediaTypeOptions.All,
      allowsEditing: false,
      aspect: [4, 3],
      quality: 1,
    });

    if (!result.cancelled) {
      console.log(result);
      this.setState({image: result.uri});
    }
  };
  submit = () =>{
    console.log("Sumbit button!");
    this.commitNewDog();
  }

  commitNewDog = ()=>{    
    var token = 'Bearer ' + this.props.token 
    var  url = this.props.Navi.URL + 'lostdogs';
    const photo = {
      uri: this.state.image,
      type: "image/jpeg",
      name: "photo.jpg"
    };

    var dog={
      breed: "dogdog",
      age: "5",
      size: "Large, very large",
      color: "Orange but a bit yellow and green dots",
      specialMark: "tattoo of you on the neck",
      name: "Cat",
      hairLength: "Long",
      tailLength: "None",
      earsType: "Big",
      behaviors: ["Angry","Sad"],
      location: {
        city:"Biała",
        district:"Small"
      },
      dateLost: "2021-03-20",
      ownerId: "1"
    }
    const data = new FormData();
    data.append("dog",JSON.stringify(dog));
    data.append('picture', photo);    
    console.log("Data form sended");

    this.props.Navi.RunOnBackend("registerNewDog",data).then((responseData)=>{
      //console.log(responseData)
      console.log("succes new dog added !")
    }).catch((x)=>
        console.log("Login Error" + (x))
      )
    return 0;




    fetch(url, {
        method: "POST",
        headers: {
          'Accept': '*/*',
          'Authorization': token,
        },
          body: data
        })
        .then(response => response.json())
        .then(response => console.log("Succed new dog was added with id: " + response.data.id))
        .catch((error) => console.error(error))
        .finally(() => {/*Loading switch to false*/});
  }

  render(){
    return(
        <View style={styles.content}>
        {/* Icon */}
        <View style={[{flexDirection: 'row', width: 300, margin: 30}]}>
                <Image source={newDog} style={[styles.Icon,{width: 150,height:150}]}/>
                <Text  style={[{ width: 200,fontSize: 30, fontWeight: 'bold', textAlignVertical: 'center'}]}>What kind of dog have you lost?</Text>
        </View>
            <SafeAreaView style={[{height: '66%',width: 230}, styles.Center]}>
                <ScrollView>
                {/* String data */}
                    <TextInput style={styles.inputtext} placeholder="Dog Name"      onChangeText={(x) => this.setState({name: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Breed"         onChangeText={(x) => this.setState({breed: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Age"           onChangeText={(x) => this.setState({age: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Size"          onChangeText={(x) => this.setState({size: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Color"         onChangeText={(x) => this.setState({color: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Special mark"  onChangeText={(x) => this.setState({specialMark: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Hair length"   onChangeText={(x) => this.setState({hairLength: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Tail Length"   onChangeText={(x) => this.setState({tailLength: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Ears type"     onChangeText={(x) => this.setState({earsType: x})}/>
                    <Text style={styles.normText}>Can you describe his two most common behaviors?</Text>
                    <TextInput style={styles.inputtext} placeholder="Behaviour 1"   onChangeText={(x) => this.setState({behaviour1: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Behaviour 2"   onChangeText={(x) => this.setState({behaviour2: x})}/>
                    <Text style={styles.normText}>When the dog was lost?</Text>
                    <TextInput style={styles.inputtext} placeholder="Data"          onChangeText={(x) => this.setState({dateLost: x})}/>
                    <Text style={styles.normText}>Localization:</Text>
                    <TextInput style={styles.inputtext} placeholder="City"          onChangeText={(x) => this.setState({LocationCity: x})}/>
                    <TextInput style={styles.inputtext} placeholder="District"      onChangeText={(x) => this.setState({LocationDistinct: x})}/>
                {/* Photos */}
                    <View style={styles.row}>
                        <TouchableOpacity onPress={() => this.makeImage()} style={[styles.circle,{width: 80, height: 80, borderRadius: 90,margin: 10, marginBottom: 0, display: 'flex',alignItems: 'center', justifyContent: 'center',}]}>
                            <Image source={camera} style={[{aspectRatio: 1, height: 45, width: 45}]}/>
                        </TouchableOpacity>
                        <TouchableOpacity onPress={() => this.pickImage()} style={[styles.circle,{width: 80, height: 80, borderRadius: 90,margin: 10, marginBottom: 0,display: 'flex',alignItems: 'center', justifyContent: 'center',}]}>
                            <Image source={gallery} style={[{aspectRatio: 1, height: 45, width: 45}]}/>
                        </TouchableOpacity>
                    </View>
                    <View style={styles.row}>
                    {
                        this.state.image!=null? 
                         <Image source={{uri: this.state.image}} style={styles.dogPic}/> 
                        : <View/>
                    }
                    </View>
                    <TouchableOpacity style={styles.SubmitButton} onPress={() => this.submit()}>
                        <Text style={styles.whiteText}>Submit</Text>
                    </TouchableOpacity>
                </ScrollView>
            </SafeAreaView>
        </View>
  )
  }
}


const styles = StyleSheet.create({
  Center:{
    marginLeft: 'auto', 
    marginRight: 'auto',
    alignSelf: 'center',
    textAlignVertical: 'center',
    },
  content: {
    height: '80%',
    alignSelf: 'center',
  },
icon:{
    resizeMode: 'contain',
    aspectRatio: 1, 
    width: 50,
    marginLeft: '35%',
  },
  dogPic:{
    height: 100,
    width: 100,
    resizeMode: 'contain',
    aspectRatio: 0.7, 
    marginLeft: '25%',
    borderRadius: 600,
  },
  row:{
    flexDirection: "row",
},
circle: {
    backgroundColor: 'white',
    alignContent: 'center',
    backgroundColor: 'black',
  },
  inputtext: {
    fontSize: 16,
    height: 30,
    width: width*0.5,
    borderColor: '#000000',
    borderWidth: 1,
    borderRadius: 5,
    paddingLeft: 5,
    marginVertical: 10,
  },
  normText:{
    fontSize: 16,
    width: width*0.5,
    paddingLeft: 5,
    marginTop: 5,
  },
  whiteText:{
    color: 'white',
    fontSize: 25,
    textAlign: 'center',
    margin: 5,
  },
  SubmitButton:{
    margin: 5,
    backgroundColor: 'black',
    width: '70%',
    alignSelf: 'center',
  },
});
