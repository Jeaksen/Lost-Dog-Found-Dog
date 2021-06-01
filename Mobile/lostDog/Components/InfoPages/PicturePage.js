import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image } from 'react-native';
import * as ImagePicker from 'expo-image-picker';

import CameraIcon from '../../Assets/camera.png';
import GalleryIcon from '../../Assets/gallery.png';
import NoCameraIcon from '../../Assets/nocamera.png';

const {width, height} = Dimensions.get("screen")


export default class PicturePage extends React.Component {

    goToNext=()=>{
        console.log("goToNext")
        this.props.ParentRef.moveToNext();
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
          this.props.ParentRef.setPicture(result.uri)
          this.goToNext()
          //this.setState({image: result.uri});
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
          //console.log(result);
          //this.setState({image: result.uri});
          this.props.ParentRef.setPicture(result.uri)
          this.goToNext()
        }
      };

  render(){
    return(
        <View style={styles.content}>
          <Text style={styles.Title}>Step 1/7 - Picture</Text>
          <View>
            <TouchableOpacity style={styles.Button} onPress={() => this.makeImage()}>
                <Image style={[styles.ButtonIcon,{marginLeft: '5%'}]} source={CameraIcon} />
                <Text style={styles.ButtonText} >Make it now</Text>
            </TouchableOpacity>

            <TouchableOpacity style={styles.Button} onPress={() => this.pickImage()}>
                <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={GalleryIcon} />
                <Text style={styles.ButtonText} >I already have it</Text>
            </TouchableOpacity>

            <TouchableOpacity style={styles.Button} onPress={() => this.goToNext()}>
                <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={NoCameraIcon} />
                <Text style={styles.ButtonText} >I don't have any</Text>
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
