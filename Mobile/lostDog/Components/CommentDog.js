import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image } from 'react-native';
import CalendarIcon from '../Assets/calendar.png';
import ClockIcon from '../Assets/clock.png';
import GpsIcon from '../Assets/gps.png';
import NormalDog from '../Assets/smallDog.png';
import Mark from '../Assets/mark.png';
import Ear from '../Assets/ear.png';
import Age from '../Assets/age.png';
import Size from '../Assets/size.png';
import Hair from '../Assets/hair.png';
import Tail from '../Assets/tail.png';
import Brain from '../Assets/brain.png';

const {width, height} = Dimensions.get("screen")
const stc = require('string-to-color');

export default class CommentDog extends React.Component {
  constructor(props) {
    super(props);
   }
   toUri = (picture) =>{
    return "data:" + picture.fileType+";base64,"+picture.data
  }
  validate=(x)=>{
    return(x=="" || x==null || x=='None')
  }


   Date=(x)=>{
       if(this.validate(x)) return (<View></View>)
       return(
        <View style={styles.Info}>
            <Image style={[styles.InfoIcon,{marginLeft: '5%'}]} source={CalendarIcon} />
            <Text style={styles.InfoText} >Lost on {x}</Text>
        </View>
       )
   }
   Time=(x)=>{
    if(this.validate(x)) return (<View></View>)
    return(
     <View style={styles.Info}>
         <Image style={[styles.InfoIcon,{marginLeft: '5%'}]} source={ClockIcon} />
         <Text style={styles.InfoText} >Around time: {x}</Text>
     </View>
    )
    }
    LocationCity=(x)=>{
        if(this.validate(x)) return (<View></View>)
        return(
        <View style={styles.Info}>
            <Image style={[styles.InfoIcon,{marginLeft: '5%'}]} source={GpsIcon} />
            <Text style={styles.InfoText} >In the city: {x}</Text>
        </View>
        )
    }
    LocationDistrict=(x)=>{
        if(this.validate(x)) return (<View></View>)
        return(
        <View style={styles.Info}>
            <Image style={[styles.InfoIcon,{marginLeft: '5%'}]} source={GpsIcon} />
            <Text style={styles.InfoText} >District: {x}</Text>
        </View>
        )
    }
    OtherInfo=(icon,title,x)=>{
        if(this.validate(x)) return (<View></View>)
        return(
        <View style={styles.Info}>
            <Image style={[styles.InfoIcon,{marginLeft: '5%'}]} source={icon} />
            <Text style={styles.InfoText} >{title}: {x}</Text>
        </View>
        )
    }
    Color=(x)=>{
        if(this.validate(x)) return (<View></View>)
        const color = stc(x);
        const maxSize=12;
        return(
            <View style={styles.Info}>
                <View style={[styles.InfoIcon,{marginLeft: '5%', backgroundColor: color, borderRadius: 10}]} />
                {
                    x.length>maxSize?
                    <Text style={styles.InfoText} >Color: {x.substring(0,maxSize)}...</Text>:
                    <Text style={styles.InfoText} >Color: {x}</Text>
                }
            </View>
            )
    }
    LongInfo=(icon,title,x)=>{
        if(this.validate(x)) return (<View></View>)
        return(
        <View style={[styles.Info,{height: 50}]}>
            <Image style={[styles.InfoIcon,{marginLeft: '5%'}]} source={icon} />
            <View>
                <Text style={[styles.InfoText, {width: '90%'}]}>{title}:</Text>
                <Text style={[styles.InfoText, {width: '90%'}]}>{x}</Text>
            </View>
        </View>
        )
    }


    found=()=>{
        this.props.Navi.swtichPage(12,this.props.item);
    }
    rejected=()=>
    {
        console.log(this.props.item.filterInfo)
        this.props.Navi.swtichPage(8,this.props.item.filterInfo);
    }
  render(){
    return(
      <View>
      {
        this.props.item.data!=null?
        <View style={styles.content}>
            <Text style={{fontWeight: 'bold', fontSize: 30, marginLeft: 0.09*width, color: '#99481f'}}>{this.props.item.data.name}</Text>
            <View style={{flexDirection: 'row', alignContent: 'center', width: 0.8*width}}>
                <Image source={{uri: this.toUri(this.props.item.data.picture)}} style={styles.dogPic}/> 
                <View>
                    {this.Date(this.props.item.data.dateLost.split("T")[0])}
                    {this.Time(this.props.item.data.dateLost.split("T")[1])}
                    {this.LocationCity(this.props.item.data.location.city)}
                    {this.LocationDistrict(this.props.item.data.location.district)}
                    {this.OtherInfo(NormalDog,"Breed",this.props.item.data.breed)}
                    {this.OtherInfo(Age,"Age",this.props.item.data.age)}
                    {this.OtherInfo(Size,"Size",this.props.item.data.size)}
                    {this.OtherInfo(Hair,"Hair",this.props.item.data.hairLength)}
                    {this.OtherInfo(Ear,"Ears",this.props.item.data.earsType)}
                    {this.OtherInfo(Tail,"Tail",this.props.item.data.tailLength)}
                    {this.LongInfo(Mark,"Special mark",this.props.item.data.specialMark)}
                    {
                        this.props.item.data.behaviors.map((e)=>{this.LongInfo(Brain,"-Behaviour",e.behvaior)})
                    }
                    {this.Color(this.props.item.data.color)}
                </View>
            </View>
        </View>
        :null
      }
        <TouchableOpacity style={[styles.Button,{backgroundColor: '#5fd67c'}]} onPress={() => this.found()} >
                    <Text style={styles.ButtonText}>Yes, this is the dog I saw!</Text>
        </TouchableOpacity>
        <TouchableOpacity style={[styles.Button,{backgroundColor: '#fe706d'}]} onPress={() => this.rejected()} >
                    <Text style={styles.ButtonText}>No, that's not it.</Text>
        </TouchableOpacity>
      </View>
  )
  }

}


const styles = StyleSheet.create({
    infoText:{
      fontSize: 15,
      margin: 2,
    },
      content: {
        margin: 30,
        alignSelf: 'center',
        justifyContent: 'center',
      },
      text:{
        marginTop: 'auto',
        marginBottom: 'auto',
        fontSize: 10,
        color: '#99481f',
        textAlign: 'center',
    },
    dogPic:{
      height: '80%',
      width: 120,
      alignSelf: 'center',
      borderRadius: 30,
    },
    Info:{
        //backgroundColor: '#feb26d',
        width: width*0.5,
        height: height*0.04,
        flexDirection: 'row',
        alignContent: 'center',
        borderRadius: 15,
    },
    InfoText:{
        marginTop: 'auto',
        marginBottom: 'auto',
        marginLeft: 15,
        fontSize: 15,
        color: '#99481f',
        alignSelf: 'flex-start',
        width: '75%',
    },
    InfoIcon:{
        width: 25,
        height:25,
        alignSelf: 'center',
        tintColor: '#99481f'
    },
    Button:{
        backgroundColor: '#feb26d',
        width: width*0.5,
        height: height*0.06,
        marginLeft: 'auto',
        marginRight: 'auto',
        marginTop: 10,
        borderRadius: 15,
    },
    ButtonText:{
        marginTop: 'auto',
        marginBottom: 'auto',
        fontSize: 15,
        color: 'white',
        textAlign: 'center',
        width: '100%',
    },
});
