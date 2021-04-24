import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity, Image,ScrollView,SafeAreaView } from 'react-native';
import found from '../Assets/found.png'
import serach from '../Assets/search.png'
const {width, height} = Dimensions.get("screen")


export default class FoundDog extends React.Component {
    state={
        // Copied from other page most of this is not usefull
        breed: "dogdog",
        age: "5",
        size: "Large",
        color: "Orange",
        specialMark: "tatto on the neck",
        name: "Cat",
        hairLength: "Long",
        tailLength: "None",
        earsType: "Short",
        behaviour1: "Angry",
        behaviour2: "Sad",
        LocationCity: "Biała",
        LocationDistinct: "Small",
        dateLost: "2021-03-20",
      }
    
      SearchButton=()=>{
        this.props.Navi.swtichPage(3);
      }
  render(){
    return(
        <View style={styles.content}>
            <View style={[{flexDirection: 'row', width: 300, margin: 20}]}>
                <Image source={found} style={[styles.Icon,{width: 150,height:150}]}/>
                <Text  style={[{ width: 200,fontSize: 30, fontWeight: 'bold', textAlignVertical: 'center'}]}> What kind of dog did you seen ?</Text>
            </View>
            <SafeAreaView style={[{height: '30%'}]}>
                <ScrollView >
                {/* String data */}
                    <TextInput style={styles.inputtext} placeholder="Breed"         onChangeText={(x) => this.setState({breed: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Age"           onChangeText={(x) => this.setState({age: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Size"          onChangeText={(x) => this.setState({size: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Color"         onChangeText={(x) => this.setState({color: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Special mark"  onChangeText={(x) => this.setState({specialMark: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Hair length"   onChangeText={(x) => this.setState({hairLength: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Tail Length"   onChangeText={(x) => this.setState({tailLength: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Ears type"     onChangeText={(x) => this.setState({earsType: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Behaviour 1"   onChangeText={(x) => this.setState({behaviour1: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Behaviour 2"   onChangeText={(x) => this.setState({behaviour2: x})}/>
                    <Text style={styles.normText}>When the dog was lost?</Text>
                    <TextInput style={styles.inputtext} placeholder="Data"          onChangeText={(x) => this.setState({dateLost: x})}/>
                    <Text style={styles.normText}>Localization:</Text>
                    <TextInput style={styles.inputtext} placeholder="City"          onChangeText={(x) => this.setState({LocationCity: x})}/>
                    <TextInput style={styles.inputtext} placeholder="District"      onChangeText={(x) => this.setState({LocationDistinct: x})}/>
                </ScrollView>
            </SafeAreaView>
            <TouchableOpacity style={[styles.Center,{margin: 20}]} onPress={() => this.SearchButton()}>
                <Image source={serach} style={[styles.Icon,{width: 80,height:80}]}/>
            </TouchableOpacity>
        </View>
  )
  }
}


const styles = StyleSheet.create({
    Center:{
        marginLeft: 'auto', 
        marginRight: 'auto',
        alignSelf: 'center',
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
});
