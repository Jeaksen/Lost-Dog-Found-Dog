import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity, Image,ScrollView,SafeAreaView } from 'react-native';
import found from '../Assets/found.png'
import serach from '../Assets/search.png'
const {width, height} = Dimensions.get("screen")


export default class FoundDog extends React.Component {
    state={
        // Copied from other page most of this is not usefull
        breed: "",
        ageFrom: "",
        ageTo: "",
        size: "",
        color: "",
        name: "",
        locationCity: "",
        locationDistrict: "",
        dateLostBefore: "",
        dateLostAfter: "",
      }
    
      SearchButton=()=>{
        data ={
          breed: this.state.breed,
          ageFrom: this.state.ageFrom,
          ageTo: this.state.ageTo,
          size: this.state.size,
          color: this.state.color,
          name: this.state.name,
          locationCity: this.state.locationCity,
          locationDistrict: this.state.locationDistrict,
          dateLostBefore: this.state.dateLostBefore,
          dateLostAfter: this.state.dateLostAfter,
        }
        console.log("Wysyłam dane: "+ data);
        this.props.Navi.swtichPage(8,data);
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
                    <TextInput style={styles.inputtext} placeholder="Name"              onChangeText={(x) => this.setState({name: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Breed"             onChangeText={(x) => this.setState({breed: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Age From"          onChangeText={(x) => this.setState({ageFrom: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Age To"            onChangeText={(x) => this.setState({ageTo: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Size"              onChangeText={(x) => this.setState({size: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Color"             onChangeText={(x) => this.setState({color: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Location city"     onChangeText={(x) => this.setState({locationCity: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Location district" onChangeText={(x) => this.setState({locationDistrict: x})}/>
                    <TextInput style={styles.inputtext} placeholder="Before lost date"  onChangeText={(x) => this.setState({dateLostBefore: x})}/>
                    <TextInput style={styles.inputtext} placeholder="After lost date"   onChangeText={(x) => this.setState({dateLostAfter: x})}/>
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
