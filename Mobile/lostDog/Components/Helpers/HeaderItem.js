import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity, Image } from 'react-native';
import AppIcon from '../../Assets/AppIcon.png'
import LoginIcon from '../../Assets/login.png'
import RegisIcon from '../../Assets/register.png'
import newDog from '../../Assets/newdog.png'
import found from '../../Assets/found.png'
import userIcon from '../../Assets/animal-care.png'
import logout from '../../Assets/logout.png'

const {width, height} = Dimensions.get("screen")
var size=100
var butMargin=10

const Icons=[
  {text: "Sign in", icon: LoginIcon},
  {text: "Sign up", icon: RegisIcon},
  {text: "FoundDog", icon: found},
  {text: "Add Dog", icon: newDog},
  {text: "User", icon: userIcon},
  {text: "logout", icon: logout},
]

export default class HeaderItem extends React.Component {

  getContent=(SearchText,size)=>
  {
    for (let iconData of Icons) 
    {
      if(iconData.text==SearchText && iconData.icon!=null)
      {
        return (<Image source={iconData.icon} style={[styles.Icon,{width: size*0.7, height: size*0.7}]}/>)
      }
    }
    return (<Text style={styles.text}>{SearchText}</Text>)
  }
  render(){
    size=this.props.size
    butMargin= this.props.butMargin
    return(
        <TouchableOpacity style={[styles.circle,{width: size, height: size, borderRadius: size/2, margin: butMargin}]} onPress={() => this.props.headerInput(this.props.id)}>
              {this.getContent(this.props.title,size)}
        </TouchableOpacity>
  )
  }
}
/*
<Text style={styles.text}>{this.props.title}</Text>
              <Image source={AppIcon} style={styles.AppIcon}/>
              */

const styles = StyleSheet.create({
  Icon:{
    resizeMode: 'contain',
    aspectRatio: 1, 
    opacity: 0.8,
    alignSelf: 'center',
    marginTop: 'auto',
    marginBottom: 'auto',
    },
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
