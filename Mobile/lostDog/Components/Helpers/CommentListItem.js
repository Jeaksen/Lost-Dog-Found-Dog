import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image } from 'react-native';

const {width, height} = Dimensions.get("screen")


export default class DogListItem extends React.Component {


  toUri = (picture) =>{
    return "data:" + picture.fileType+";base64,"+picture.data
  }

  render(){
    //console.log(this.props.item)
    return(
        <View style={styles.content}>
            <View style={{flexDirection: 'row',alignContent: 'center',alignSelf: 'center', width: '80%', padding: 15}}>
              <View>
                <Text>{this.props.item.location.city}</Text>
                <Text>{this.props.item.location.district}</Text>
                <Text> </Text>
              </View>
              <View style={{width: '50%'}}/>
              <View>
                <Text style={{alignSelf: 'flex-end'}}>{this.props.item.author.name}</Text>
                <Text style={{alignSelf: 'flex-end'}}>{this.props.item.author.email}</Text>
                <Text style={{alignSelf: 'flex-end'}}>{this.props.item.author.phoneNumber}</Text>
              </View>
            </View>
            <View style={{flexDirection: 'row',alignContent: 'center',}}>
              {/*Picture*/
                this.props.item.picture.data!=null? 
                <Image source={{uri: this.toUri(this.props.item.picture)}} style={styles.dogPic}/> 
                : <View/>
              }
              <Text style={styles.longText} >{this.props.item.text}</Text>
            </View>
        </View>
  )
  }

}


const styles = StyleSheet.create({
    content: {
      borderWidth: 1,
        borderColor: '#feb26d',
        borderRadius: 90,
        padding: 10,
        margin: 10,
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

    },
    longText:{
      width: '50%',
      margin: 5,
    }
});
