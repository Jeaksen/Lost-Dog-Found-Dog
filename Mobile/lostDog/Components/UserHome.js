import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity, Image } from 'react-native';
import userIcon from '../Assets/animal-care.png'
import SettingsIcon from '../Assets/settings.png'
import dogIcon from '../Assets/dog.png'
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
            <View style={[{flexDirection: 'row', width: 200, margin: 20}, styles.Center]}>
                <Image source={userIcon} style={[styles.Icon,{width: 150,height:150}]}/>
                <Text  style={[{fontSize: 30, fontWeight: 'bold', textAlignVertical: 'center'}]}> Szymon</Text>
            </View>
            <Text style={[{fontSize:20, margin: 20}]}>Content of that page will be implemented in the futhure Sprints</Text>
            <View style={[{flexDirection: 'row', width: 200}, styles.Center]}>
                <TouchableOpacity style={styles.Center}>
                    <Image source={SettingsIcon} style={[styles.Icon,{width: 80,height:80}]}/>
                </TouchableOpacity>
                <TouchableOpacity style={styles.Center} onPress={() => this.ListOfDogs()}>
                    <Image source={dogIcon} style={[styles.Icon,{width: 80,height:80}]}/>
                </TouchableOpacity>
            </View>

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
}
});
