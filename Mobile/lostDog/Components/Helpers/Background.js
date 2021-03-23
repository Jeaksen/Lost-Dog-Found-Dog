import * as React from 'react';
import { View, StyleSheet,Image, Text,ImageBackground,Dimensions,Animated  } from 'react-native';
import backImage from '../../Assets/mobileBackground_noBones.jpg';
import bone from '../../Assets/bone.png';

const {width, height} = Dimensions.get("screen")
const speed=10000
const delta=1
var currentDelta=delta

export default class Background extends React.Component {
    state={
        IdleAnim: new Animated.Value(0),
    }
    
    componentDidMount(){
        this.idle()
      }
    idle () {
        currentDelta = -currentDelta;
        Animated.timing(
          this.state.IdleAnim,
          {
            toValue: currentDelta,
            duration: speed,
            useNativeDriver: true,
          }
        ).start(() => {
          this.idle()
        })
      }

  render(){
    const idleAnim={
        transform: [
            {
            translateY:this.state.IdleAnim,
            }
        ],
    };
    return(
  <ImageBackground style={styles.BackImage} source={backImage}>
        <Animated.View style={[styles.row, {marginLeft: '70%'},{transform: [{translateY: this.state.IdleAnim.interpolate({inputRange: [-1, 1],outputRange: [-50, 50]})}]}]}>
            <Image source={bone} style={styles.bone}/>
        </Animated.View>
        <Animated.View style={[styles.row, {marginLeft: '60%'},{transform: [{translateY: this.state.IdleAnim.interpolate({inputRange: [-1, 1],outputRange: [25, -25]})}]}]}>
            <Image source={bone} style={styles.bone}/>
        </Animated.View>
        <Animated.View style={[styles.row, {marginLeft: '40%'},{transform: [{translateY: this.state.IdleAnim.interpolate({inputRange: [-1, 1],outputRange: [-100, 100]})}]}]}>
            <Image source={bone} style={styles.bone}/>
        </Animated.View>
        <Animated.View style={[styles.row, {marginLeft: '20%'},{transform: [{translateY: this.state.IdleAnim.interpolate({inputRange: [-1, 1],outputRange: [-100, 50]})}]}]}>
            <Image source={bone} style={styles.bone}/>
        </Animated.View>
        <Animated.View style={[styles.row, {marginLeft: '00%'},{transform: [{translateY: this.state.IdleAnim.interpolate({inputRange: [-1, 1],outputRange: [100, -10]})}]}]}>
            <Image source={bone} style={styles.bone}/>
        </Animated.View>
  </ImageBackground>
  )
  }
}


const styles = StyleSheet.create({
    BackImage: {
        flex: 1,
        width: width,
        height: height,
        resizeMode: "cover",
        justifyContent: "center",
        position: 'absolute'
      },
      row:{
          flex: 1,
          flexDirection: "row",
          alignItems: 'center',
      },
      bone:{
        resizeMode: 'contain',
        aspectRatio: 1.5, 
        opacity: 0.6,
      }
});
