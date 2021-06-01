import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity, Image } from 'react-native';
import userIcon from '../Assets/animal-care.png'
import SettingsIcon from '../Assets/settings.png'
import dogIcon from '../Assets/dog.png'
import logout2 from '../Assets/logout2.png'
import NormalDog from '../Assets/smallDog.png';

import newDog from '../Assets/newdog.png'
import found from '../Assets/found.png'
import shelter from '../Assets/animal-shelter.png'

const {width, height} = Dimensions.get("screen")


export default class UserHome extends React.Component {

    constructor(props)
    {
        super(props);
    }
    
    ListOfDogs=()=>{
        this.props.Navi.swtichPage(3,null);
    }

  render(){
    return(
        <View style={styles.content}>
          <View style={[{flexDirection: 'row', width: 0.8*width, marginBottom: 40}]}>
                    <Image source={userIcon} style={[styles.Icon,{width: 100, height:100}]}/>
                    <Text  style={[{ width: '70%', fontSize: 25, fontWeight: 'bold', textAlignVertical: 'center', color: '#99481f'}]}>Hello This is your home page.</Text>
          </View>

            <TouchableOpacity style={styles.Button} onPress={() =>  this.props.Navi.swtichPage(3)}>
                <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={NormalDog} />
                <Text style={styles.ButtonText} >Show my dogs</Text>
            </TouchableOpacity>
            <TouchableOpacity style={styles.Button} onPress={() =>  this.props.Navi.swtichPage(4)}>
                <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={newDog} />
                <Text style={styles.ButtonText} >I lost my dog</Text>
            </TouchableOpacity>
            <TouchableOpacity style={styles.Button} onPress={() =>  this.props.Navi.swtichPage(7)}>
                <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={found} />
                <Text style={styles.ButtonText} >I saw a dog</Text>
            </TouchableOpacity>
            <TouchableOpacity style={styles.Button} onPress={() =>  this.props.Navi.swtichPage(9)}>
                <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={shelter} />
                <Text style={styles.ButtonText} >Show me the list of shelters</Text>
            </TouchableOpacity>
            <TouchableOpacity style={styles.Button} onPress={() =>  this.props.Navi.swtichPage(1)}>
                <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={logout2} />
                <Text style={styles.ButtonText} >Log out</Text>
            </TouchableOpacity>
        </View>
  )
  }
}


const styles = StyleSheet.create({
  Icon:{
    resizeMode: 'contain',
    aspectRatio: 1, 
    alignSelf: 'center',
    marginTop: 'auto',
    marginBottom: 'auto',
    },
  Center:{
    marginLeft: 'auto', 
    marginRight: 'auto',
    alignSelf: 'center',
    textAlignVertical: 'center',
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
Button:{
  backgroundColor: '#feb26d',
  width: width*0.5,
  height: height*0.06,
  margin: 10,
  marginLeft: 'auto',
  marginRight: 'auto',
  flexDirection: 'row',
  alignContent: 'center',
  borderRadius: 15,
},
ButtonText:{
  marginTop: 'auto',
  marginBottom: 'auto',
  fontSize: 15,
  color: 'white',
  textAlign: 'center',
  width: '75%',
},
ButtonIcon:{
  width: 35,
  height:35,
  alignSelf: 'center',
}
});
