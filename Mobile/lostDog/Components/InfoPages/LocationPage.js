import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image, Alert } from 'react-native';
import * as Permissions from 'expo-permissions'
import * as Location from 'expo-location'

import GpsIcon from '../../Assets/gps.png';
import WrittingIcon from '../../Assets/writting.png';
import SkipIcon from '../../Assets/skip.png';

const {width, height} = Dimensions.get("screen")


export default class LocationPage extends React.Component {

    _getLocation = async () =>{
        const{status}  = Permissions.askAsync(Permissions.Location);

        if(status !=='granted')
        {
            console.log("Permission not granted !");
            Alert.alert(
                "No permissions",
                "I can not take your localization becouse I don't have permission"
              );
        }

        const userLocation = await Location.getCurrentPositionAsync()
        console.log(userLocation);

    }
    goToNext=()=>{
        this.props.ParentRef.moveToNext();
    }

  render(){
    return(
        <View style={styles.content}>
          <Text style={styles.Title}>Step 2/7 - Location</Text>
          <View>
            <TouchableOpacity style={styles.Button} onPress={() => this._getLocation()}>
                <Image style={[styles.ButtonIcon,{marginLeft: '5%'}]} source={GpsIcon} />
                <Text style={styles.ButtonText} >Get my location</Text>
            </TouchableOpacity>

            <TouchableOpacity style={styles.Button} onPress={() => this.goToNext()}>
                <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={WrittingIcon} />
                <Text style={styles.ButtonText} >Enter Manually</Text>
            </TouchableOpacity>

            <TouchableOpacity style={styles.Button} onPress={() => this.goToNext()}>
                <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={SkipIcon} />
                <Text style={styles.ButtonText} >Skip</Text>
            </TouchableOpacity>
          </View>
        </View>
  )
  }
}


const styles = StyleSheet.create({
    content: {
        height: '100%',
        margin: 50,
        alignSelf: 'center',
        marginVertical: 'auto',
    },
    Title:{
        fontSize: 20,
        marginTop: 10,
        alignSelf: 'center',
        color: '#99481f',
    },
    Button:{
        backgroundColor: '#feb26d',
        width: width*0.5,
        height: height*0.06,
        margin: 20,
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
