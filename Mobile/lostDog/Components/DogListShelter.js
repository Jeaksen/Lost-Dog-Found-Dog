import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity ,ScrollView,SafeAreaView,FlatList,Image} from 'react-native';
import * as ImagePicker from 'expo-image-picker';
import DogListItemShelter from './Helpers/DogListItemShelter';
import ShelterIcon from '../Assets/animal-shelter.png'

const {width, height} = Dimensions.get("screen")


export default class DogListShelter extends React.Component {
  state={
    info: " normal ",
    image: null,

    DogList: [],
  }

  getDogList = ()=>{
    this.props.Navi.RunOnBackend("getShelterDogList",{id: this.props.item.id}).then((responseData)=>{
      //console.log(responseData)
      this.setState({DogList: responseData});
      console.log("succes list of dogs is ready")
    }).catch((x)=>
        console.log("Login Error" + (x))
      )
  }

  loadingDogListFailed=()=>{

  }

  pickImage = async () => {
    let result = await ImagePicker.launchImageLibraryAsync({
      mediaTypes: ImagePicker.MediaTypeOptions.All,
      allowsEditing: false,
      aspect: [4, 3],
      quality: 1,
    });

    if (!result.cancelled) {
      //console.log(result);
      this.setState({image: result.uri});
    }
  };

  getDogInfo = ()=>{
    //console.log("getDogInfo Button");
  }

  constructor(props) {
    super(props);
    this.getDogList();
   }

  dogSelected=(item)=>{
    console.log("Dog is selected " + item.id);
    
    this.props.Navi.swtichPage(15,{dog: item, shelter: this.props.item});
  }
  render(){
    return(
        <View style={styles.content}>
            <View style={[{flexDirection: 'row', width: 0.8*width, margin: 0}]}>
                    <Image source={ShelterIcon} style={{width: 100, height:100, margin: 20,}}/>
                    <Text  style={[{ width: '70%', fontSize: 25, fontWeight: 'bold', textAlignVertical: 'center', color: '#99481f'}]}>Dogs in shelter {this.props.item.name}</Text>
                </View>
           <FlatList
            data={this.state.DogList.length>0 ? this.state.DogList : []}
        
            renderItem={({item}) => <DogListItemShelter item={item} dogSelected={this.dogSelected}/>}
            keyExtractor={(item) => item.id.toString()}
           />
        </View>
  )
  }
}

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
    marginTop:30,
    margin: 15,
    height: '90%',
    alignSelf: 'center',
    justifyContent: 'center',
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
