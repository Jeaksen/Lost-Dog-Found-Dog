import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity, Image,ScrollView,SafeAreaView } from 'react-native';
import * as ImagePicker from 'expo-image-picker';
import addDogIcon from '../Assets/addDogIcon.png';
import camera from '../Assets/camera.png';
import gallery from '../Assets/gallery.png';


const {width, height} = Dimensions.get("screen")


export default class RegisterNewDog extends React.Component {
  state={
    image: null,
    dogName: "MydOG",
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

    const data = new FormData();
    data.append('breed', this.state.breed);
    data.append('age', this.state.age);
    data.append('size', this.state.size);
    data.append('color', this.state.color);
    data.append('specialMark', this.state.specialMark);
    data.append('name', this.state.name);
    data.append('hairLength', this.state.hairLength);
    data.append('tailLength', this.state.tailLength);
    data.append('earsType', this.state.earsType);
    data.append('behaviors', this.state.behaviour1);
    data.append('behaviors', this.state.behaviour2);
    data.append('location.City', this.state.LocationCity);
    data.append('location.District', this.state.LocationDistinct);
    data.append('dateLost', this.state.dateLost);
    data.append('ownerId', this.props.id);
    data.append('picture', photo);    
    console.log("Data form sended");
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
        <View style={[styles.row, {margin: 5}]}>
            <Image source={addDogIcon} style={styles.icon} tintColor='black'/>
        </View>
        
            <SafeAreaView>
                <ScrollView >
                {/* String data */}
                    <TextInput style={styles.inputtext} placeholder="Dog Name"      onChangeText={(x) => this.setState({dogName: x})}/>
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
