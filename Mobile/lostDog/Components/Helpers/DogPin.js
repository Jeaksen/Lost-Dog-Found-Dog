import * as React from 'react';
import { StyleSheet, View, Image } from 'react-native';
import pinPic from '../../Assets/found.png'

export default class DogPin extends React.Component {

    toUri = (picture) =>{
        console.log("data:" + picture.fileType+";base64,"+picture.data)
        return "data:" + picture.fileType+";base64,"+picture.data
      }

  render(){
    console.log("this.props.picture:")
    console.log(this.props.picture)
    return(
        <View style={styles.dogPin}>
                <Image source={pinPic} style={styles.dogPin}/>
                {
                  this.props.picture?
                  <View style={{height: 540, width: 50, position:"absolute"}}>
                    <Image source={{uri: this.props.picture.uri}} style={styles.circle}/>
                  </View>:
                  <View/>
                }
        </View>)
  }
}

const styles = StyleSheet.create({
    dogPin:{
        width:50,
        height:60,
      },
      circle: {
        height: 50,
        width: 50,
        borderRadius: 50,
      }
});
