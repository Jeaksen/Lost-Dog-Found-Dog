import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image } from 'react-native';

const {width, height} = Dimensions.get("screen")


export default class DogListItem extends React.Component {


  toUri = (picture) =>{
    return "data:" + picture.fileType+";base64,"+picture.data
  }

  render(){
    console.log(this.props.item)
    return(
        <View>
            <Text>{this.props.key}</Text>
            <Text>{this.props.id}</Text>
            <Text>{this.props.text}</Text>
            <Text>{this.props.location}</Text>
        </View>
  )
  }

}


const styles = StyleSheet.create({
    content: {
        alignSelf: 'center',
        flex: 1,
        flexDirection: 'row',
      },
      text:{
        marginTop: 'auto',
        marginBottom: 'auto',
        fontSize: 10,
        color: 'black',
        textAlign: 'center',
    },
    dogPic:{
      height: 140,
      width: 140,
      resizeMode: 'contain',
      aspectRatio: 0.7, 
      borderRadius: 600,
    },
    statusBas:{

    }
});
