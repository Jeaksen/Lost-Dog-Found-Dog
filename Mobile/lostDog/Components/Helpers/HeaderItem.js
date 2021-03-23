import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity } from 'react-native';

const {width, height} = Dimensions.get("screen")
var size=100
var butMargin=10

export default class HeaderItem extends React.Component {
  render(){
    size=this.props.size
    butMargin= this.props.butMargin
    return(
        <TouchableOpacity style={[styles.circle,{width: size, height: size, borderRadius: size/2, margin: butMargin}]} onPress={() => this.props.headerInput(this.props.id)}>
              <Text style={styles.text}>{this.props.title}</Text>
        </TouchableOpacity>
  )
  }
}


const styles = StyleSheet.create({
    circle: {
        backgroundColor: 'white',
        alignContent: 'center',
      },
      text:{
        marginTop: 'auto',
        marginBottom: 'auto',
        fontSize: 10,
        color: 'black',
        textAlign: 'center',
    }
});
