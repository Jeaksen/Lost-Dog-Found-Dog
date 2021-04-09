import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity ,ScrollView,SafeAreaView,FlatList} from 'react-native';
import * as ImagePicker from 'expo-image-picker';
import DogListItem from './Helpers/DogListItem';

const {width, height} = Dimensions.get("screen")


export default class DogList extends React.Component {
  state={
    info: " normal ",
    image: null,

    DogList: [],
  }

  getDogList = ()=>{
    console.log("getDogList Button");
    console.log('Bearer ' + this.props.token);
    var token = 'Bearer ' + this.props.token 
    fetch(this.props.Navi.URL + 'lostdogs', {
        method: 'GET', 
        headers: {
            'Content-Type': 'application/json',
            'Accept': '*/*',
            'Authorization': token,
        },
        //body: JSON.stringify({})

    })
    .then(response => {
      if (response.status == 404 || response.status == 401) {
          /* Wrong input reaction */
          console.log("Wrong input Reaction");
          console.log(response.status);
          return null;
      }
      else if (response.status == 200) {
          return response.json();
        }
      else{
        /* Wrong input reaction */
        console.log("Wrong input Reaction");
        console.log(response.status);
        return null;
      }
      })
      .then(responseData => {
        if (responseData != null) 
        {
          console.log(" SUCCESS !")
          
          this.setState({DogList: responseData});
        } 
      })
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

  commitNewDog = ()=>{    
    var token = 'Bearer ' + this.props.token 
    var  url = this.props.Navi.URL + 'lostdogs';
    const photo = {
      uri: this.state.image,
      type: "image/jpeg",
      name: "photo.jpg"
    };
    console.log("V: 3.0");
    const data = new FormData();
    data.append('breed', 'dogdog');
    data.append('age', '5');
    data.append('size', 'Large');
    data.append('color', 'Orange');
    data.append('specialMark', 'tattoo of you on the neck');
    data.append('name', 'Cat');
    data.append('hairLength', 'Long');
    data.append('tailLength', 'None');
    data.append('earsType', 'Short');
    data.append('behaviors', 'Angry');
    data.append('behaviors', 'Sad');
    data.append('location.City', 'Biała');
    data.append('location.District', 'Small');
    data.append('dateLost', '2021-03-20');
    data.append('ownerId', '1');
    data.append('Image', photo);    
    console.log("Data created succesfully");
    fetch(url, {
        method: "POST",
        headers: {
          'Accept': '*/*',
          'Authorization': token,
        },
          body: data
        })
        .then(response => response.json())
        .then(response => console.log(response.status))
        .catch((error) => console.error(error))
        .finally(() => setTimeout(() => console.log("TIMEOUT")));
  }

  getDogInfo = ()=>{
    console.log("getDogInfo Button");
  }

  constructor(props) {
    super(props);
    this.getDogList();
    console.log(this.state.DogList);
    console.log("LENGTH: " + this.state.DogList.length);
   }
  render(){
    return(
        <View style={styles.content}>
            <FlatList style={{marginBottom: 140}}
            data={this.state.DogList.length > 0 ? this.state.DogList : []}
            renderItem={({ item }) => <DogListItem id={item.data.id}/>}
            keyExtractor={(item) => item.data.id.toString()}
            />
        </View>
  )
  }
}

/*
            <Text style={styles.Title}>{this.state.info}</Text>
            <Text style={styles.Title}>Id: {this.props.id}</Text>
            <TouchableOpacity style={styles.loginButton} onPress={() => this.getDogList()}>
              <Text style={styles.logintext}>get lost dog list</Text>
            </TouchableOpacity>
            */
const styles = StyleSheet.create({
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
  content: {
    marginHorizontal: 30,
    height: '100%',
    alignSelf: 'center',
    justifyContent: 'center',
    marginVertical: 'auto',
  },
  loginButton:{
    marginTop: 20,
    backgroundColor: 'black',
    width: width*0.2,
    height: height*0.05,
    marginLeft: 'auto',
    marginRight: 'auto',
},
logintext:{
    marginTop: 'auto',
    marginBottom: 'auto',
    fontSize: 15,
    color: 'white',
    textAlign: 'center',
},
Title:{
  marginBottom: 50,
  fontSize: 20,
  textAlign: 'center',
  fontWeight: 'bold',
},
});
